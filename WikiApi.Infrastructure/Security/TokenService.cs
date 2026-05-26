using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WikiApi.Application.Interfaces;

namespace WikiApi.Infrastructure.Security;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<string> GenerateTokenAsync(string userName, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        // Recupera a chave primeiro e valida se ela existe antes de transformá-la em bytes
        var secretKey = _configuration["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new InvalidOperationException("A chave JWT (Jwt:Key) não foi configurada no appsettings.json ou nas variáveis de ambiente.");
        }

        var key = Encoding.ASCII.GetBytes(secretKey);

        // Garante que o tempo de expiração seja lido com segurança
        if (!double.TryParse(_configuration["Jwt:ExpireMinutes"], out var expireMinutes))
        {
            expireMinutes = 60; // Valor padrão de contingência caso falhe a leitura
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, role)
            }),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"] ?? "60")),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
