using System.ComponentModel.DataAnnotations;

namespace WikiApi.Application.Dtos;

public record UpdateArticleRequest(
    [Required] int Id,
    [Required, StringLength(100)] string Title,
    [Required, StringLength(5000)] string Content,
    [StringLength(200)] string Tags,
    [Required, StringLength(50)] string Category
);