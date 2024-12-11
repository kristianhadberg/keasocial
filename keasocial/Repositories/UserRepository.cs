using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
using Neo4j.Driver;

namespace keasocial.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDriver _driver;

    public UserRepository(IDriver driver)
    {
        _driver = driver;
    }

    public async Task<User> GetAsync(int id)
    {
        await using var session = _driver.AsyncSession();
        var query = @"
            MATCH (u:User)
            WHERE u.UserId = $id
            RETURN u.UserId AS UserId, u.Name AS Name, u.Email AS Email";

        var cursor = await session.RunAsync(query, new { id });

        var record = await cursor.SingleAsync();

        if (record == null)
        {
            return null;
        }

        return new User
        {
            UserId = record["UserId"].As<int>(),
            Name = record["Name"].As<string>(),
            Email = record["Email"].As<string>(),
        };
    }

    public async Task<List<User>> GetAsync()
    {
        /*return await _keasocialDbContext.Users.ToListAsync();*/
        return null;
    }

    public async Task<User> Create(User user)
    {
        /*await _keasocialDbContext.Users.AddAsync(user);
        await _keasocialDbContext.SaveChangesAsync();

        return user;*/
        return null;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        /*return await _keasocialDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);*/
        return null;
    }

    public async Task<User> Login(LoginRequestDto loginRequestDto)
    {
        /*return await _keasocialDbContext.Users.FirstOrDefaultAsync(u => u.Email == loginRequestDto.Email);*/
        return null;
    }
}