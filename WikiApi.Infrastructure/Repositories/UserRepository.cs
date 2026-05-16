using Microsoft.EntityFrameworkCore;
using WikiApi.Application.Interfaces.Repositories;
using WikiApi.Domain.Entities;
using WikiApi.Infrastructure.Data;

namespace WikiApi.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly WikiDbContext _context;

    public UserRepository(WikiDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task CreateAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);

        await _context.SaveChangesAsync();
    }
}
