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

    public async Task<List<PostDto>> GetAsync()
    {
        return await _postRepository.GetAsync();
    }
    
    public async Task<Post> CreateAsync(PostCreateDto postCreateDto)
    {
        ValidatePostContent(postCreateDto.Content);
        
        var post = new Post
        {
            UserId = postCreateDto.UserId,
            Content = postCreateDto.Content,
            CreatedAt = DateTime.UtcNow,
            LikeCount = postCreateDto.LikeCount,
            
        };
        return await _postRepository.CreateAsync(post);

    }

    public async Task<Post> UpdateAsync(int id, PostUpdateDto postUpdateDto, int userId)
    {
        ValidatePostContent(postUpdateDto.Content);
        
        Post post = await _postRepository.GetAsync(id);
        
        if (post == null)
        {
            throw new KeyNotFoundException($"Post with ID {id} not found.");
        }
        
        if (post.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this post.");
        }
        
        post.Content = postUpdateDto.Content;
        post.LikeCount = postUpdateDto.LikeCount;
        var updatedPost = await _postRepository.UpdateAsync(id, post);
        return updatedPost;
    }

    public async Task<Post> DeleteAsync(int userId, int postId)
    {
        var post = await _postRepository.GetAsync(postId);

        if (post == null)
        {
            throw new KeyNotFoundException($"Post with id: {postId} does not exist.");
        }

        if (post.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this post.");
        }
        
        return await _postRepository.DeleteAsync(postId);
    }

    public async Task<bool> AddPostLikeAsync(int userId, int postId)
    {
        var postExists = await _postRepository.GetAsync(postId);

        if (postExists == null)
        {
            throw new KeyNotFoundException($"Post with id: {postId} does not exist.");
        }
        
        var success = await _postRepository.AddPostLikeAsync(userId, postId);

        if (!success)
        {
            throw new BadHttpRequestException("You have already liked this post.");
        }

        return true;
    }

    public async Task<List<PostLikeView>> GetPostLikesAsync(int postId)
    {
        return await _postRepository.GetPostLikesAsync(postId);
    }

    public async Task<List<PostDto>> GetMostLikedPostsAsync()
    {
        return await _postRepository.GetMostLikedPostsAsync();
    }

    public void ValidatePostContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content) || content.Length is < 5 or > 100)
        {
            throw new ArgumentException("Content must be between 5 and 100 characters long.");
            
        }
    }
    
}