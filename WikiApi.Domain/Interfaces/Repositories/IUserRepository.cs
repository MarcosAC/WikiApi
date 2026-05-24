using WikiApi.Domain.Entities;

namespace WikiApi.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUserNameAsync(string username);
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
}