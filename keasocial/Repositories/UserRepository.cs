using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
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

    public async Task<User> Create(User user)
    {
        await _keasocialDbContext.Users.AddAsync(user);
        await _keasocialDbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _keasocialDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> Login(LoginDto loginDto)
    {
        return await _keasocialDbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
    }
}