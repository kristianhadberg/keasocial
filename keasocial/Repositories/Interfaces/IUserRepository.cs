using keasocial.Models;

namespace keasocial.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> GetAsync(int id);
    Task<List<User>> GetAsync();
}