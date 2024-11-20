using keasocial.Dto;
using keasocial.Models;

namespace keasocial.Services.Interfaces;

public interface IUserService
{
    Task<User> GetAsync(int id);
    Task<List<User>> GetAsync();
    Task<User> Create(UserCreateDto userCreateDto);
    Task<User> Login(LoginDto loginDto);
}