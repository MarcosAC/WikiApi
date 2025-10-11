using WikiApi.Application.Interfaces;
using WikiApi.Application.Dtos;
using WikiApi.Domain.Entities;

namespace WikiApi.Application.Services;

public class ArticleService
{
    private readonly IArticleRepository _repository;

    public ArticleService(IArticleRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ArticleDto>> GetAllAsync(string? search = null, string? tag = null)
    {
        var list = await _repository.GetAllAsync(search, tag);

        return list.Select(article =>
                            new ArticleDto(
                                article.Id,
                                article.Title,
                                article.Content,
                                article.Tags,
                                article.Category,
                                article.CreatedAt,
                                article.UpdateAt
                            ));
    }

    public async Task<ArticleDto?> GetByIdAsync(int id)
    {
        var article = await _repository.GetByIdAsync(id);

        return article == null ? null : new ArticleDto(
                                            article.Id,
                                            article.Title,
                                            article.Content,
                                            article.Tags,
                                            article.Category,
                                            article.CreatedAt,
                                            article.UpdateAt
                                        );
    }

    public async Task<ArticleDto> CreateAsync(CreateArticleRequest request)
    {
        var article = new Article(request.Title, request.Content, request.Tags, request.Category);

        await _repository.AddAsync(article);

        return new ArticleDto(
                    article.Id,
                    article.Title,
                    article.Content,
                    article.Tags,
                    article.Category,
                    article.CreatedAt,
                    article.UpdateAt
                );
    }

    public async Task UpdateAsync(UpdateArticleRequest request)
    {
        var article = await _repository.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException("Artigo não encontrado");

        article.Update(request.Title, request.Content, request.Tags, request.Category);

        await _repository.UpdateAsync(article);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}