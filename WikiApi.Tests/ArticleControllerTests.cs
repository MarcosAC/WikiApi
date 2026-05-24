using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using WikiApi.Api.Controllers;
using WikiApi.Application.Dtos;
using WikiApi.Application.Services;
using WikiApi.Domain.Entities;
using WikiApi.Infrastructure.Data;
using WikiApi.Infrastructure.Repositories;
using Xunit;

namespace WikiApi.Tests;

public class ArticleControllerTests
{
    /*  Este teste de integração para o ArticlesController verifica se as operações CRUD 
     *  estão funcionando corretamente com um banco de dados em memória. */

    private readonly ArticlesController _articlesController;
    private readonly WikiDbContext _wikiDbContext;
    private readonly ArticleService _articleService;

    public ArticleControllerTests()
    {
        var options = new DbContextOptionsBuilder<WikiDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _wikiDbContext = new WikiDbContext(options);
        var repository = new ArticleRepository(_wikiDbContext);

        // 1. Criamos o Mock do IHttpContextAccessor para o Controller/Service de teste
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, "AdminUser") };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        httpContextAccessorMock.Setup(accessor => accessor.HttpContext).Returns(httpContext);

        // 2. Instanciamos o serviço passando o mock do contexto
        _articleService = new ArticleService(repository, httpContextAccessorMock.Object);
        _articlesController = new ArticlesController(_articleService);

        // 3. Simula o contexto HTTP diretamente no Controller para validações internas de rota/user se houver
        _articlesController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // CORREÇÃO: O construtor do Article agora pede o autor na semeadura de dados ("AdminUser")        
        _wikiDbContext.Articles.Add(new Article(
            "DotNet Test",
            "Content about .NET",
            "dotnet,backend",
            "Programming",
            "AdminUser"
        ));

        _wikiDbContext.SaveChanges();
    }

    [Fact]
    public async Task GetAll_ReturnsArticlesList()
    {
        var result = await _articlesController.GetAll(null, null) as OkObjectResult;
        var articles = result?.Value as IEnumerable<ArticleDto>;

        Assert.NotNull(articles);
        Assert.Single(articles);
    }

    [Fact]
    public async Task GetById_ReturnsArticle_WhenExists()
    {
        var existingArticle = _wikiDbContext.Articles.First();
        var result = await _articlesController.GetById(existingArticle.Id) as OkObjectResult;
        var article = result?.Value as ArticleDto;

        Assert.NotNull(article);
        Assert.Equal(existingArticle.Id, article.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenDoesNotExist()
    {
        var result = await _articlesController.GetById(999) as NotFoundResult;

        Assert.NotNull(result);
    }

    [Fact]
    public async Task Create_AddsArticleSuccessfully()
    {
        var request = new CreateArticleRequest(
            "New Article",
            "Some content",
            "test,api",
            "Test"
        );

        var result = await _articlesController.Create(request) as CreatedAtActionResult;
        var article = result?.Value as ArticleDto;

        Assert.NotNull(article);
        Assert.Equal("New Article", article.Title);
        Assert.Equal(2, _wikiDbContext.Articles.Count());
    }

    [Fact]
    public async Task Update_ModifiesArticleSuccessfully()
    {
        var existing = _wikiDbContext.Articles.First();
        var updateRequest = new UpdateArticleRequest(
            existing.Id,
            "Updated Title",
            existing.Content,
            existing.Tags,
            existing.Category
        );

        var result = await _articlesController.Update(existing.Id, updateRequest);
        var updatedArticle = _wikiDbContext.Articles.Find(existing.Id);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal("Updated Title", updatedArticle?.Title);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var updateRequest = new UpdateArticleRequest(
            999,
            "Mismatch",
            "Content",
            "tag",
            "cat"
        );

        var result = await _articlesController.Update(1, updateRequest);
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Delete_RemovesArticleSuccessfully()
    {
        var existing = _wikiDbContext.Articles.First();
        var result = await _articlesController.Delete(existing.Id);

        Assert.IsType<NoContentResult>(result);
        Assert.Empty(_wikiDbContext.Articles);
    }
}