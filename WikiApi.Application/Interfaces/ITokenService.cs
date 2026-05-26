namespace WikiApi.Application.Interfaces;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(string userName, string role);
}
