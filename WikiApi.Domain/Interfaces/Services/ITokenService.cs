namespace WikiApi.Domain.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(string userName, string role);
}
