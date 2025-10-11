using Microsoft.EntityFrameworkCore;
using WikiApi.Application.Interfaces;
using WikiApi.Domain.Entities;
using WikiApi.Infrastructure.Data;

namespace WikiApi.Infrastructure.Repositories;

public class ArticleRepository : IArticleRepository
{
    private readonly WikiDbContext _context;

    public ArticleRepository(WikiDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Article article)
    {
        _context.Articles.Add(article);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var article = await _context.Articles.FindAsync(id);

        if (article != null)
        {
            _context.Articles.Remove(article);

            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Article>> GetAllAsync(string? search = null, string? tag = null)
    {
        var query = _context.Articles.AsQueryable();

        // ILIKE usa provider Npgsql; aqui usamos Contains como fallback simples
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(article => EF.Functions.ILike(article.Title, $"%{search}%") ||
                                           EF.Functions.ILike(article.Content, $"%{search}%"));
        }

        if (!string.IsNullOrWhiteSpace(tag))
        {
            query = query.Where(article => EF.Functions.ILike(article.Tags, $"%{tag}%"));
        }

        return await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
    }

    public async Task<Article?> GetByIdAsync(int id)
    {
        return await _context.Articles.FindAsync(id);
    }

    public async Task UpdateAsync(Article article)
    {
        _context.Articles.Update(article);

        await _context.SaveChangesAsync();
    }
}