using System.ComponentModel.DataAnnotations;
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
        
        if (string.IsNullOrWhiteSpace(postCreateDto.Content) || postCreateDto.Content.Length < 5 || postCreateDto.Content.Length > 100)
        {
            throw new ArgumentException("Content must be between 5 and 100 characters long.");
        }
        
        var post = new Post
        
        {
            UserId = postCreateDto.UserId,
            Content = postCreateDto.Content,
            CreatedAt = DateTime.UtcNow,
            LikeCount = postCreateDto.LikeCount,
            
        };
        return await _postRepository.CreateAsync(post);

    }
    
    public async Task<Post> UpdateAsync(int id, PostUpdateDto postUpdateDto)
    {
        
        if (string.IsNullOrWhiteSpace(postUpdateDto.Content) || postUpdateDto.Content.Length < 5 || postUpdateDto.Content.Length > 100)
        {
            throw new ArgumentException("Content must be between 5 and 100 characters long.");
        }
        
        Post post = await _postRepository.GetAsync(id);
        
        if (post == null)
        {
            throw new KeyNotFoundException($"Post with ID {id} not found.");
        }
        post.Content = postUpdateDto.Content;
        post.LikeCount = postUpdateDto.LikeCount;
        var updatedPost = await _postRepository.UpdateAsync(id, post);
        return updatedPost;
    }
    
    public async Task<Post> DeleteAsync(int id)
    {
        return await _postRepository.DeleteAsync(id);
    }
}