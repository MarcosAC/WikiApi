namespace WikiApi.Application.Dtos;

public record ArticleDto(int Id, string Title, string Content, string Tags, string Category, DateTime CreatedAt, DateTime? UpdateAt);