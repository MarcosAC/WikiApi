using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using WikiApi.Application.Dtos.Requests;
using WikiApi.Application.Dtos.Responses;
using WikiApi.Application.Interfaces.Services;
using WikiApi.Domain.Entities;
using WikiApi.Domain.Interfaces.Repositories;

namespace WikiApi.Application.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ArticleService(IArticleRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<ArticleResponse>> GetAllAsync(string? search = null, string? tag = null)
    {
        var list = await _repository.GetAllAsync(search, tag);

        return list.Select(static article => new ArticleResponse
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            Tags = article.Tags,
            CreatedAt = article.CreatedAt,
            UpdatedAt = article.UpdatedAt,
            AuthorName = article.Author
        });
    }

    public async Task<ArticleResponse?> GetByIdAsync(int id)
    {
        var article = await _repository.GetByIdAsync(id);

        if (article == null) return null;

        return new ArticleResponse
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            Tags = article.Tags,
            CreatedAt = article.CreatedAt,
            UpdatedAt = article.UpdatedAt,
            AuthorName = article.Author
        };
    }

    public async Task<ArticleResponse> CreateAsync(CreateArticleRequest request)
    {
        var autorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(autorName))
        {
            throw new UnauthorizedAccessException("Usuário não identificado no token.");
        }

        // Instancia a entidade de Domínio pura
        var article = new Article(request.Title, request.Content, request.Tags, request.Category, autorName);

        await _repository.AddAsync(article);

        // Retorna o DTO correspondente
        return new ArticleResponse
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            Tags = article.Tags,
            CreatedAt = article.CreatedAt,
            UpdatedAt = article.UpdatedAt,
            AuthorName = article.Author
        };
    }

    public async Task UpdateAsync(UpdateArticleRequest request)
    {
        var article = await _repository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException("Artigo não encontrado");

        // Executa a regra de negócio de alteração dentro da própria entidade do domínio
        article.Update(request.Title, request.Content, request.Tags, request.Category);

        await _repository.UpdateAsync(article);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}