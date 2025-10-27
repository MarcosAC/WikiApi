using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        _articleService = new ArticleService(repository);
        _articlesController = new ArticlesController(_articleService);

        // Popular dados iniciais
        _wikiDbContext.Articles.Add(new Article(
            "DotNet Test",
            "Content about .NET",
            "dotnet,backend",
            "Programming"
        ));

        _wikiDbContext.SaveChanges();
    }

    [Fact]
    public async Task GetAllReturnsArticlesList()
    {
        var result = await _articlesController.GetAll(null, null) as OkObjectResult;
        var articles = result?.Value as IEnumerable<ArticleDto>;

        Assert.NotNull(articles);
        Assert.Single(articles);
    }

    [Fact]
    public async Task GetByIdReturnsArticlesWhenExists()
    {
        var existingArticle = _wikiDbContext.Articles.First();
        var result = await _articlesController.GetById(existingArticle.Id) as OkObjectResult;
        var article = result?.Value as ArticleDto;

        Assert.NotNull(article);
        Assert.Equal(existingArticle.Id, article.Id);
    }

    [Fact]
    public async Task GeyByIdReturnsNotFoundWhenDoesNotExust()
    {
        var result = await _articlesController.GetById(999) as NotFoundResult;

        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateAddsArticleSucessfully()
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