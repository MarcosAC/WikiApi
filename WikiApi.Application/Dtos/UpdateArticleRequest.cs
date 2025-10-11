namespace WikiApi.Application.Dtos;

public record UpdateArticleRequest(int Id, string Title, string Content, string Tags, string Category);