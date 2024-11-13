using keasocial.Dto;
using keasocial.Models;

namespace keasocial.Services.Interfaces;

public interface IPostService
{
    Task<Post> GetAsync(int id);
    Task<List<Post>> GetAsync();
    
    Task<Post> CreateAsync(PostCreateDto postCreate);
    
    Task<Post> UpdateAsync(int id, PostUpdateDto postCreate);
    
    Task<Post> DeleteAsync(int id);
    
}