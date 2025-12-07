using WikiApi.Application.Interfaces;
using WikiApi.Application.Dtos;
using WikiApi.Domain.Entities;
using WikiApi.Application.Mappers;
using WikiApi.Application.Dtos.Responses;

namespace WikiApi.Application.Services;

public class ArticleService
{
    private readonly IArticleRepository _repository;

    public ArticleService(IArticleRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ArticleResponse>> GetAllAsync(string? search = null, string? tag = null)
    {
        var articles = await _repository.GetAllAsync(search, tag);

        return articles.ToResponseList();
    }

    public async Task<ArticleResponse?> GetByIdAsync(int id)
    {
        var article = await _repository.GetByIdAsync(id);

        return article?.ToResponse();
    }

    public async Task<ArticleResponse> CreateAsync(CreateArticleRequest request)
    {
        var article = new Article(request.Title, request.Content, request.Tags, request.Category);

        await _repository.AddAsync(article);

        return article.ToResponse();
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