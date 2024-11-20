using keasocial.Dto;
using keasocial.Models;

namespace keasocial.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> GetAsync(int id);
    Task<List<User>> GetAsync();
    Task<User> Create(User user);
    Task<User> GetByEmailAsync(string email);
    Task<User> Login(LoginDto loginDto);
}