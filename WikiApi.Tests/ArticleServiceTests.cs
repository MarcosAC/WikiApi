using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using WikiApi.Application.Dtos.Requests;
using WikiApi.Application.Services;
using WikiApi.Domain.Entities;
using WikiApi.Domain.Interfaces.Repositories;
using Xunit;

public class ArticleServiceTests
{
    /*   Este teste unitário verifica se o método CreateAsync do ArticleService está funcionando corretamente.
     1. O repositório é mocado para simular a adição de um artigo sem realmente acessar um banco de dados.
     2. O HttpContextAccessor é mocado para simular um usuário logado, permitindo que o serviço obtenha o nome do autor.
     3. O serviço é criado com as dependências mocadas e o método CreateAsync é chamado com uma solicitação de criação de artigo.
     4. Asserções são feitas para verificar se o título do artigo retornado é correto, se o autor é o esperado e se o método AddAsync do repositório foi chamado exatamente uma vez.
     Este teste garante que a lógica de criação de artigos no ArticleService está funcionando conforme o esperado, incluindo a interação com o repositório e a obtenção do nome do autor a partir do contexto HTTP. */
    [Fact]
    public async Task CreateAsyncShouldAddAticle()
    {
        // 1. Moca o repositório
        var repoMock = new Mock<IArticleRepository>();
        repoMock.Setup(repository => repository.AddAsync(It.IsAny<Article>())).Returns(Task.CompletedTask);

        // 2. Moca o HttpContextAccessor para simular o usuário logado
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, "TestAuthor") };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        httpContextAccessorMock.Setup(accessor => accessor.HttpContext).Returns(httpContext);

        // 3. Injeta ambos no serviço
        var service = new ArticleService(repoMock.Object, httpContextAccessorMock.Object);

        var articleRequest = new CreateArticleRequest("Title A", "Content", "tag1,tag2", "Tutorial");

        // Act
        var articleDto = await service.CreateAsync(articleRequest);

        // Assert
        Assert.Equal("Title A", articleDto.Title);
        Assert.Equal("TestAuthor", articleDto.AuthorName);
        repoMock.Verify(repository => repository.AddAsync(It.IsAny<Article>()), Times.Once);
    }
}