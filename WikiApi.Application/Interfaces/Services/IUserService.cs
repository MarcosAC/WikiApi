using WikiApi.Application.Dtos.Requests;

namespace WikiApi.Domain.Interfaces.Services;

public interface IUserService
{
    Task<bool> RegisterAsync(RegisterRequest request);
}
