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
        if (login.UserName == "admin" && login.Password == "123456")
        {
            var token = _tokenService.GenerateTokenAsync(login.UserName, "Admin");
            return Ok(new { Token = token });
        }    
        
        return Unauthorized();
    }
}
