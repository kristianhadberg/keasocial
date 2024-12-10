using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
using MongoDB.Driver;

namespace keasocial.Repositories;

public class MongoUserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public MongoUserRepository(IMongoDatabase database)
    {
        _users = database.GetCollection<User>("Users");
    }

    public async Task<User> GetAsync(int id)
    {
        return await _users.Find(u => u.UserId == id).FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetAsync()
    {
        return await _users.Find(_ => true).ToListAsync();
    }

    public async Task<User> Create(User user)
    {
        await _users.InsertOneAsync(user);
        return user;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task<User> Login(LoginRequestDto loginRequestDto)
    {
        return await _users.Find(u => u.Email == loginRequestDto.Email).FirstOrDefaultAsync();
    }
}