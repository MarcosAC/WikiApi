using Moq;
using Xunit;
using WikiApi.Application.Interfaces;
using WikiApi.Domain.Entities;
using WikiApi.Application.Services;
using WikiApi.Application.Dtos;
using FluentAssertions;

public class ArticleServiceTests
{
    [Fact]
    public async Task CreateAsuncShouldAddAticle()
    {
        var repoMock = new Mock<IArticleRepository>();
        repoMock.Setup(repository => repository.AddAsync(It.IsAny<Article>())).Returns(Task.CompletedTask);

        var service = new ArticleService(repoMock.Object);

        var articleRequest = new CreateArticleRequest("Title A", "Content", "tag1,tag2", "Tutorial");
        var articleDto = await service.CreateAsync(articleRequest);

        articleDto.Title.Should().Be("Title A");
        repoMock.Verify(repository => repository.AddAsync(It.IsAny<Article>()), Times.Once);

    }
}