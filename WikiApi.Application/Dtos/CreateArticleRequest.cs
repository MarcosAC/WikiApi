namespace WikiApi.Application.Dtos;

public record CreateArticleRequest(string Title, string Content, string Tags, string Category);