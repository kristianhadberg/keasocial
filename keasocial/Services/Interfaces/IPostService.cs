using keasocial.Dto;
using keasocial.Models;

namespace keasocial.Services.Interfaces;

public interface IPostService
{
    Task<Post> GetAsync(string uuid);
    Task<List<PostDto>> GetAsync();
    
    Task<Post> CreateAsync(PostCreateDto postCreate);
    
    Task<Post> UpdateAsync(string uuid, PostUpdateDto postCreate, string userUuid);
    
    Task<Post> DeleteAsync(string postUuid, string userUuid);
    Task<bool> AddPostLikeAsync(string postUuid, string userUuid);

}