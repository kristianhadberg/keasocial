using keasocial.Dto;
using keasocial.Models;

namespace keasocial.Services.Interfaces;

public interface IUserService
{
    Task<User> GetAsync(string uuid);
    Task<List<User>> GetAsync();
    Task<User> Create(UserCreateDto userCreateDto);
    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
}