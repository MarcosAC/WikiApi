using Microsoft.AspNetCore.Identity;
using WikiApi.Application.Dtos.Requests;
using WikiApi.Application.Interfaces.Repositories;
using WikiApi.Domain.Entities;
using WikiApi.Domain.Interfaces.Services;

namespace WikiApi.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher<string> _passwordHasher;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = new PasswordHasher<string>();
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userRepository.GetByUserNameAsync(request.UserName);

        if (existingUser != null) return false;

        // Instanciar um objeto temporário sem senha apenas para passar ao gerador de hash nativo
        // O gerador do .NET precisa da instância para aplicar segurança baseada nas propriedades do objeto
        var newUser = new User(request.UserName, "placeholder", request.Role);

        // Gerar o hash real e seguro a partir da senha em texto limpo
        var secureHash = _passwordHasher.HashPassword(request.UserName, request.Password);

        newUser.UpdatePassword(secureHash);

        await _userRepository.CreateAsync(newUser);

        return true;

    }
}
