using keasocial.Models;
using keasocial.Dto;

namespace keasocial.Repositories.Interfaces;

public interface ICommentRepository
{
    Task <CommentDto> CreateAsync(Comment comment);
    Task <Comment> GetAsync(string commentId);
    Task <List<CommentDto>> GetAsync();
    Task <IEnumerable<CommentDto>> GetByPostIdAsync(int postId);
    Task<Comment> UpdateAsync(string commentId, Comment comment);
    Task <Comment> DeleteAsync(string commentId);
    
    Task<bool> AddCommentLikeAsync(int userId, string commentId, int postId);
}