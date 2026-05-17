using Microsoft.AspNetCore.Identity;
using WikiApi.Application.Interfaces.Repositories;
using WikiApi.Application.Interfaces.Services;
using WikiApi.Domain.Entities;
using WikiApi.Domain.Interfaces.Services;

namespace WikiApi.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(IUserRepository userRepository, ITokenService tokenService) 
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<string?> AuthenticateAsync(string userName, string password)
        {
            var user = await _userRepository.GetByUserNameAsync(userName);

            if (user == null) return null;
            
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            if (result == PasswordVerificationResult.Failed) return null;

            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                var newHash = _passwordHasher.HashPassword(user, password);

                user.UpdatePassword(newHash);

                await _userRepository.UpdateAsync(user);
            }

            return await _tokenService.GenerateTokenAsync(user.UserName, user.Role);
        }
    }
}
