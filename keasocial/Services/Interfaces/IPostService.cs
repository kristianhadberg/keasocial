using keasocial.Models;

namespace keasocial.Services.Interfaces;

public interface IPostService
{
    Task<Post> GetAsync(int id);
    Task<List<Post>> GetAsync();
}