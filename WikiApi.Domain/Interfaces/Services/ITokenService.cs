namespace WikiApi.Domain.Interfaces.Services;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(string userName, string role);
}
