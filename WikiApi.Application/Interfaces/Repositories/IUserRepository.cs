using WikiApi.Domain.Entities;

namespace WikiApi.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUserNameAsync(string userName);
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
}
