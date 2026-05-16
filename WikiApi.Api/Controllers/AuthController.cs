using Microsoft.AspNetCore.Mvc;
using WikiApi.Application.Dtos.Requests;
using WikiApi.Domain.Interfaces.Services;

namespace WikiApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest login)
    {
        var token = await _tokenService.GenerateTokenAsync(login.UserName, login.Password);

        if (token == null)
        {
            return Unauthorized(new {message = "Usuário ou senha inválidos"});
        }

        return Ok(new { Token = token });
    }
}
