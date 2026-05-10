using System.ComponentModel.DataAnnotations;

namespace WikiApi.Application.Dtos;

public record CreateArticleRequest(
    [Required, StringLength(100)] string Title,
    [Required, StringLength(5000)] string Content,
    [StringLength(200)] string Tags,
    [Required, StringLength(50)] string Category
);