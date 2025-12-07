using WikiApi.Application.Dtos.Responses;
using WikiApi.Domain.Entities;

namespace WikiApi.Application.Mappers;

public static class ArticleMapper
{
    public static ArticleResponse ToResponse(this Article article)
    {
        return new ArticleResponse
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            Tags = article.Tags,
            Category = article.Category,
            CreatedAt = article.CreatedAt,
            UpdateAt = article.UpdateAt
        };
    }
    
    public static IEnumerable<ArticleResponse> ToResponseList(this IEnumerable<Article> articles)
    {
        return articles.Select(article => article.ToResponse());
    }
}