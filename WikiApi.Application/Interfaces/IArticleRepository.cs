using WikiApi.Domain.Entities;

namespace WikiApi.Application.Interfaces;

public interface IArticleRepository
{
    Task<IEnumerable<Article>> GetAllAsync(string? search = null, string? tag = null);
    Task<Article?> GetByIdAsync(int id);
    Task AddAsync(Article article);
    Task UpdateAsync(Article article);
    Task DeleteAsync(int id);
}