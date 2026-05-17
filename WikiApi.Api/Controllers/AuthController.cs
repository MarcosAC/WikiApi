using Microsoft.AspNetCore.Mvc;
using WikiApi.Application.Dtos.Requests;
using WikiApi.Application.Interfaces.Services;
using WikiApi.Domain.Interfaces.Services;

namespace WikiApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest login)
    {
        var token = await _authService.AuthenticateAsync(login.UserName, login.Password);

        if (token == null)
        {
            return Unauthorized(new {message = "Usuário ou senha inválidos"});
        }

        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest register)
    {
        var isRegistered = await _userService.RegisterAsync(register);

        if (!isRegistered)
        {
            return BadRequest(new {message = "Nome de usuário já existe"});
        }

        return Ok(new {message = "Usuário registrado com sucesso"});
    }
}
