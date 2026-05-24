using WikiApi.Domain.Interfaces;
using WikiApi.Domain.Dtos;
using WikiApi.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace WikiApi.Domain.Services;

public class IArticleService
{
    private readonly IArticleRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IArticleService(IArticleRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
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
                                article.Author,
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
                                            article.Author,
                                            article.CreatedAt,
                                            article.UpdateAt
                                        );
    }

    public async Task<ArticleDto> CreateAsync(CreateArticleRequest request)
    {
        var autorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(autorName))
        {
            throw new UnauthorizedAccessException("Usuário não identificado no token.");
        }

        var article = new Article(request.Title, request.Content, request.Tags, request.Category, autorName);

        await _repository.AddAsync(article);

        return new ArticleDto(
                    article.Id,
                    article.Title,
                    article.Content,
                    article.Tags,
                    article.Category,
                    article.Author,
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