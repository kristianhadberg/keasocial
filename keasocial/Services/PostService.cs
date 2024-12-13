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

    public async Task<Post> GetAsync(string uuid)
    {
        return await _postRepository.GetAsync(uuid);
    }

    public async Task<List<Post>> GetAsync()
    {
        return await _postRepository.GetAsync();
    }
    
    public async Task<Post> CreateAsync(PostCreateDto postCreateDto)
    {
        ValidatePostCreateDto(postCreateDto);
        
        var post = new Post
        {
            Content = postCreateDto.Content,
            CreatedAt = DateTime.UtcNow,
            LikeCount = postCreateDto.LikeCount,
        };
        return await _postRepository.CreateAsync(post, postCreateDto.UserUuid);

    }

    public async Task<Post> UpdateAsync(string postUuid, PostUpdateDto postUpdateDto, string userUuid)
    {
        ValidatePostUpdateDto(postUpdateDto);

        Post post = await _postRepository.GetAsync(postUuid);

        if (post == null)
        {
            throw new KeyNotFoundException($"Post with ID {postUuid} not found.");
        }

        var isUserAuthorized = await _postRepository.IsUserAuthorizedToChangePost(userUuid, postUuid);
        if (!isUserAuthorized)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this post.");
        }

        post.Content = postUpdateDto.Content;
        post.LikeCount = postUpdateDto.LikeCount;
        var updatedPost = await _postRepository.UpdateAsync(postUuid, post);
        return updatedPost;
    }

    public async Task<Post> DeleteAsync(string postUuid, string userUuid)
    {
        var post = await _postRepository.GetAsync(postUuid);

        if (post == null)
        {
            throw new KeyNotFoundException($"Post with id: {postUuid} does not exist.");
        }

        var isUserAuthorized = await _postRepository.IsUserAuthorizedToChangePost(userUuid, postUuid);
        if (!isUserAuthorized)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this post.");
        }
        
        return await _postRepository.DeleteAsync(postUuid);
    }

    public async Task<bool> AddPostLikeAsync(string postUuid, string userUuid)
    {
        var postExists = await _postRepository.GetAsync(postUuid);

        if (postExists == null)
        {
            throw new KeyNotFoundException($"Post with id: {postUuid} does not exist.");
        }

        var success = await _postRepository.AddPostLikeAsync(postUuid, userUuid);
        if (!success)
        {
            throw new BadHttpRequestException("You have already liked this post.");
        }

        return true;
    }

    public void ValidatePostCreateDto(PostCreateDto postCreateDto)
    {
        
        if (string.IsNullOrWhiteSpace(postCreateDto.Content) || postCreateDto.Content.Length < 5 || postCreateDto.Content.Length > 100)
        {
            throw new ArgumentException("Content must be between 5 and 100 characters long.");
        }
    }
    
    private void ValidatePostUpdateDto(PostUpdateDto postUpdateDto)
    {
        if (string.IsNullOrWhiteSpace(postUpdateDto.Content) || postUpdateDto.Content.Length < 5 || postUpdateDto.Content.Length > 100)
        {
            throw new ArgumentException("Content must be between 5 and 100 characters long.");
        }
    }
}