using Microsoft.AspNetCore.Mvc;
using WikiApi.Application.Dtos;
using WikiApi.Application.Services;

namespace WikiApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly ArticleService _articleService;

    public ArticlesController(ArticleService articleService)
    {
        _articleService = articleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromBody] string? search, [FromQuery] string? tag)
    {
        var listArticles = await _articleService.GetAllAsync(search, tag);

        return Ok(listArticles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var article = await _articleService.GetByIdAsync(id);

        return article == null ? NotFound() : Ok(article);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateArticleRequest request)
    {
        var article = await _articleService.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = article.Id }, article);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateArticleRequest request)
    {
        if (id != request.Id) return BadRequest();

        await _articleService.UpdateAsync(request);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _articleService.DeleteAsync(id);

        return NoContent();
    }   
}