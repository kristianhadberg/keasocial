using keasocial.Models;

namespace keasocial.Services.Interfaces;

public interface IUserService
{
    Task<User> GetAsync(int id);
    Task<List<User>> GetAsync();
}