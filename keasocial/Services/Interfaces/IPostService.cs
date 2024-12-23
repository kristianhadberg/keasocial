using keasocial.Dto;
using keasocial.Models;

namespace keasocial.Services.Interfaces;

public interface IPostService
{
    Task<Post> GetAsync(int id);
    Task<List<PostDto>> GetAsync();
    
    Task<Post> CreateAsync(PostCreateDto postCreate);
    
    Task<Post> UpdateAsync(int id, PostUpdateDto postCreate, int userId);
    
    Task<Post> DeleteAsync(int userId, int postId);
    Task<bool> AddPostLikeAsync(int userId, int postId);
    Task<List<PostLikeView>> GetPostLikesAsync(int postId);
    Task<List<PostDto>> GetMostLikedPostsAsync();

}