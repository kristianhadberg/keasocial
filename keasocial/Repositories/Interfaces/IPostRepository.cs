using keasocial.Models;

namespace keasocial.Repositories.Interfaces;

public interface IPostRepository
{
    Task<Post> GetAsync(int id);
    Task<List<Post>> GetAsync();
}