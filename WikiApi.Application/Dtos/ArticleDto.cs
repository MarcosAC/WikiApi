namespace WikiApi.Application.Dtos.Requests;

public record ArticleDto(
    int Id, 
    string Title,
    string Content,
    string Tags, 
    string Category, 
    string Author, 
    DateTime CreatedAt,
    DateTime? UpdatedAt
);