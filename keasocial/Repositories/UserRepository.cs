using keasocial.Models;
using Microsoft.EntityFrameworkCore;

namespace keasocial.Repositories;

public class UserRepository : IUserRepository
{
    private readonly KeasocialDbContext _keasocialDbContext;

    public UserRepository(KeasocialDbContext keasocialDbContext)
    {
        _keasocialDbContext = keasocialDbContext;
    }

    public async Task<User> GetAsync(int id)
    {
        return await _keasocialDbContext.Users.FindAsync(id);
    }

    public async Task<List<User>> GetAsync()
    {
        return await _keasocialDbContext.Users.ToListAsync();
    }
}