using WikiApi.Application.Dtos.Requests;

namespace WikiApi.Application.Interfaces.Services;

public interface IUserService
{
    Task<bool> RegisterAsync(RegisterRequest request);
}
