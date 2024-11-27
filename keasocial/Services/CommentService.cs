using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
using keasocial.Services.Interfaces;

namespace keasocial.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;

    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }
    
    public async Task<Comment> GetAsync(int id)
    {
        return await _commentRepository.GetAsync(id);
    }
    
    public async Task<List<CommentDto>> GetAsync()
    {
        return await _commentRepository.GetAsync();
    }

    public Task<IEnumerable<Comment>> GetByPostIdAsync(int postId)
    {
        throw new NotImplementedException();
    }

    public Task<Comment> UpdateAsync(int commentId, CommentUpdateDto comment)
    {
        throw new NotImplementedException();
    }

    public Task<Comment> DeleteAsync(int commentId)
    {
        throw new NotImplementedException();
    }


    public async Task<Comment> CreateAsync(CommentCreateDto commentCreateDto)
    {
        if (string.IsNullOrWhiteSpace(commentCreateDto.Content) || commentCreateDto.Content.Length < 5 || commentCreateDto.Content.Length > 200)
        {
            throw new ArgumentException("Content must be between 5 and 200 characters.");
        }

        var comment = new Comment
        {
            PostId = commentCreateDto.PostId,
            UserId = commentCreateDto.UserId,
            Content = commentCreateDto.Content,
            CreatedAt = DateTime.UtcNow,
            LikeCount = commentCreateDto.LikeCount
        };

        return await _commentRepository.CreateAsync(comment);
    }
    
    public async Task<Comment> UpdateAsync(int id, CommentUpdateDto commentUpdateDto, int userId)
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
    
    public async Task<Comment> DeleteAsync(int commentId, int userId)
    {
        var comment = await _commentRepository.GetAsync(commentId);
        
        if (comment == null)
        {
            throw new ArgumentException($"Comment with id: {commentId} not found.");
        }
        
        if (comment.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can only delete your own comments.");
        }
        
        return await _commentRepository.DeleteAsync(commentId);
    }
    
    public async Task<bool> AddCommentLikeAsync(int userId, int commentId)
    {
        var commentExists = await _commentRepository.GetAsync(commentId);

        if (commentExists == null)
        {
            throw new KeyNotFoundException($"Comment with ID {commentId} not found.");
        }
        
        var success = await _commentRepository.AddCommentLikeAsync(userId, commentId);
        
        if (!success)
        {
            throw new BadHttpRequestException("You have already liked this comment.");
        }

        return true;
    }



    public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId)
    {
        return await _commentRepository.GetByPostIdAsync(postId);
    }
}
