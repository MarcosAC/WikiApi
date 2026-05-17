using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WikiApi.Application.Dtos;
using WikiApi.Application.Services;
using WikiApi.Application.Validators;

namespace WikiApi.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly ArticleService _articleService;

    public ArticlesController(ArticleService articleService)
    {
        _articleService = articleService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? tag)
    {
        var listArticles = await _articleService.GetAllAsync(search, tag);

        return Ok(listArticles);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var article = await _articleService.GetByIdAsync(id);

        return article == null ? NotFound() : Ok(article);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateArticleRequest request)
    {
        var validator = new CreateArticleRequestValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        var article = await _articleService.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = article.Id }, article);
    }

    [Authorize(Roles = "Admin")]    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateArticleRequest request)
    {
        var validator = new UpdateArticleRequestValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        if (id != request.Id) return BadRequest();

        await _articleService.UpdateAsync(request);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _articleService.DeleteAsync(id);

        return NoContent();
    }   
}