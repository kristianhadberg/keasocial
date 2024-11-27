using keasocial.Dto;
using keasocial.Models;

namespace keasocial.Services.Interfaces;


public interface ICommentService
{
    Task<Comment> CreateAsync(CommentCreateDto comment);
    Task<Comment> GetAsync(int commentId);
    Task<List<CommentDto>> GetAsync();
    Task<IEnumerable<Comment>> GetByPostIdAsync(int postId);
    Task<Comment> UpdateAsync(int commentId, CommentUpdateDto comment, int userId);
    Task <Comment> DeleteAsync(int commentId, int userId);
    
    Task<bool> AddCommentLikeAsync(int userId, int commentId);
    Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId);
    
}