using keasocial.Dto;
using keasocial.Models;

namespace keasocial.Services.Interfaces;


public interface ICommentService
{
    Task<CommentDto> CreateAsync(CommentCreateDto comment, int postId);
    Task<Comment> GetAsync(string commentId, int postId);
    Task<List<CommentDto>> GetAsync();
    Task<IEnumerable<CommentDto>> GetByPostIdAsync(int postId);
    Task<Comment> UpdateAsync(string commentId, CommentUpdateDto comment, int userId);
    Task<Comment> DeleteAsync(string commentId, int userId, int postId);
    Task<bool> AddCommentLikeAsync(int userId, string commentId, int postId);
    
}