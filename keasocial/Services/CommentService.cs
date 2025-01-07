using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
using keasocial.Services.Interfaces;
using MongoDB.Bson;

namespace keasocial.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;

    public CommentService(ICommentRepository commentRepository, IPostRepository? postRepository)
    {
        _commentRepository = commentRepository;
        _postRepository = postRepository;
    }
    
    public async Task<Comment> GetAsync(string commentId, int postId)
    {
        var comment = await _commentRepository.GetAsync(commentId);
        if (comment == null || comment.PostId != postId)
        {
            throw new ArgumentException("Comment not found or does not belong to the specified post.");
        }

        return comment;
    }

    

    public async Task<List<CommentDto>> GetAsync()
    {
        return await _commentRepository.GetAsync();
    }

    public async Task<IEnumerable<CommentDto>> GetByPostIdAsync(int postId)
    {
        // Ensure the post exists
        var post = await _postRepository.GetAsync(postId);
        if (post == null)
        {
            throw new ArgumentException($"Post with ID {postId} does not exist.");
        }

        // Fetch and return comments
        return await _commentRepository.GetByPostIdAsync(postId);
    }


    public async Task<CommentDto> CreateAsync(CommentCreateDto commentCreateDto, int postId)
    {
        if (string.IsNullOrWhiteSpace(commentCreateDto.Content) || 
            commentCreateDto.Content.Length < 5 || 
            commentCreateDto.Content.Length > 200)
        {
            throw new ArgumentException("Content must be between 5 and 200 characters.");
        }

        var post = await _postRepository.GetAsync(postId);
        if (post == null)
        {
            throw new ArgumentException($"Post with ID {postId} does not exist.");
        }
        
        var comment = new Comment
        {
            Id = ObjectId.GenerateNewId().ToString(),
            PostId = postId,
            UserId = commentCreateDto.UserId,
            Content = commentCreateDto.Content,
            CreatedAt = DateTime.UtcNow,
            LikeCount = 0 
        };
        
        await _postRepository.AddEmbeddedCommentAsync(comment, postId);

        var createdComment = await _commentRepository.CreateAsync(comment);

        return new CommentDto()
        {
            CommentId = createdComment.CommentId,
            UserId = createdComment.UserId,
            Content = createdComment.Content,
            CreatedAt = createdComment.CreatedAt,
            LikeCount = createdComment.LikeCount
        };
        
        
    }


    
    public async Task<Comment> UpdateAsync(string id, CommentUpdateDto commentUpdateDto, int userId)
    {
        if (string.IsNullOrWhiteSpace(commentUpdateDto.Content) || commentUpdateDto.Content.Length < 5 || commentUpdateDto.Content.Length > 200)
        {
            throw new ArgumentException("Content must be between 5 and 200 characters.");
        }
        
        Comment comment = await _commentRepository.GetAsync(id);
        
        if (comment == null)
        {
            throw new ArgumentException("Comment not found.");
        }
        
        if (comment.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can only update your own comments.");
        }

        comment.Content = commentUpdateDto.Content;
        comment.LikeCount = commentUpdateDto.LikeCount;
        var updatedComment = await _commentRepository.UpdateAsync(id, comment);
        return updatedComment;
    }
    
    public async Task<Comment> DeleteAsync(string commentId, int postId, int userId)
    {
        var comment = await _commentRepository.GetAsync(commentId);

        if (comment == null || comment.PostId != postId)
        {
            throw new ArgumentException("Comment not found or does not belong to the specified post.");
        }
        
        if (comment.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can only delete your own comments.");
        }

        return await _commentRepository.DeleteAsync(commentId);
    }
    

    public async Task<bool> AddCommentLikeAsync(int userId, string commentId, int postId)
    {
        var commentExists = await _commentRepository.GetAsync(commentId);

        if (commentExists == null)
        {
            throw new KeyNotFoundException($"Comment with ID {commentId} not found.");
        }
        
        var success = await _commentRepository.AddCommentLikeAsync(userId, commentId, postId);
        
        if (!success)
        {
            throw new BadHttpRequestException("You have already liked this comment.");
        }

        return true;
    }
    public async Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(int postId)
    {
        return await _commentRepository.GetByPostIdAsync(postId);
    }
}
