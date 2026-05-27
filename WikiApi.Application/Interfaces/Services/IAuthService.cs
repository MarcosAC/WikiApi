namespace WikiApi.Application.Interfaces.Services;

public interface IAuthService
{
    Task<string?> AuthenticateAsync(string userName, string password);
}
