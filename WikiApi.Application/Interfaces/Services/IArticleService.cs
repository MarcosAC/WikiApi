using WikiApi.Application.Dtos;
using WikiApi.Application.Dtos.Responses;

namespace WikiApi.Application.Interfaces.Services;

public interface IArticleService
{
    Task<IEnumerable<ArticleResponse>> GetAllAsync(string? search, string? tag);
    Task<ArticleResponse?> GetByIdAsync(int id);

    // Os métodos de escrita continuam recebendo as Requests
    Task<ArticleResponse> CreateAsync(CreateArticleRequest request);
    Task UpdateAsync(UpdateArticleRequest request);
    Task DeleteAsync(int id);
}