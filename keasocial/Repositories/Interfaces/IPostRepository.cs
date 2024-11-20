using keasocial.Dto;
using keasocial.Models;

namespace keasocial.Repositories.Interfaces;

public interface IPostRepository
{
    Task<Post> GetAsync(int id);
    Task<List<PostDto>> GetAsync();
    Task<Post> UpdateAsync(int id, Post post);
    
    Task<Post> CreateAsync(Post post);
    
    Task<Post> DeleteAsync(int id);
    
    
}