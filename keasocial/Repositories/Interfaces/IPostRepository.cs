using keasocial.Dto;
using keasocial.Models;

namespace keasocial.Repositories.Interfaces;

public interface IPostRepository
{
    Task<Post> GetAsync(string uuid);
    Task<List<PostDto>> GetAsync();
    Task<Post> UpdateAsync(string uuid, Post post);
    
    Task<Post> CreateAsync(Post post, string userUuid);
    
    Task<Post> DeleteAsync(string uuid);
    Task<bool> AddPostLikeAsync(string postUuid, string userUuid);
    Task<bool> IsUserAuthorizedToChangePost(string userUuid, string postUuid);


}