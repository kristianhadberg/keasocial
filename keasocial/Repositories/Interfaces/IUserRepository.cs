using keasocial.Dto;
using keasocial.Models;

namespace keasocial.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> GetAsync(string uuid);
    Task<List<User>> GetAsync();
    Task<User> Create(User user);
    Task<User> GetByEmailAsync(string email);
    Task<User> Login(LoginRequestDto loginRequestDto);
}