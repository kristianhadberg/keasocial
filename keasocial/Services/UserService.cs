using keasocial.Models;
using keasocial.Repositories;
using keasocial.Repositories.Interfaces;
using keasocial.Services.Interfaces;

namespace keasocial.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> GetAsync(int id)
    {
        return await _userRepository.GetAsync(id);
    }

    public async Task<List<User>> GetAsync()
    {
        return await _userRepository.GetAsync();
    }
}