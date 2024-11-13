using keasocial.Models;
using keasocial.Dto;
using keasocial.Repositories.Interfaces;
using keasocial.Services.Interfaces;

namespace keasocial.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;

    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<Post> GetAsync(int id)
    {
        return await _postRepository.GetAsync(id);
    }

    public async Task<List<Post>> GetAsync()
    {
        return await _postRepository.GetAsync();
    }
    
    public async Task<Post> CreateAsync(PostCreateDto postCreateDto)
    {
        var post = new Post
        {
            UserId = postCreateDto.UserId,
            Content = postCreateDto.Content,
            CreatedAt = postCreateDto.CreatedAt,
            LikeCount = postCreateDto.LikeCount,
        };
        return await _postRepository.CreateAsync(post);
    }
    
    public async Task<Post> UpdateAsync(int id, PostUpdateDto postUpdateDto)
    {
        Post post = await _postRepository.GetAsync(id);
        post.Content = postUpdateDto.Content;
        post.CreatedAt = postUpdateDto.CreatedAt;
        post.LikeCount = postUpdateDto.LikeCount;
        
        var updatedPost = await _postRepository.UpdateAsync(id, post);
        return updatedPost;
    }
    
    public async Task<Post> DeleteAsync(int id)
    {
        return await _postRepository.DeleteAsync(id);
    }
}