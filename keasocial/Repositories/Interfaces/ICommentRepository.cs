using keasocial.Models;
using keasocial.Dto;

namespace keasocial.Repositories.Interfaces;

public interface ICommentRepository
{
    Task <Comment> CreateAsync(Comment comment);
    Task <Comment> GetAsync(int commentId);
    Task <List<CommentDto>> GetAsync();
    Task <IEnumerable<Comment>> GetByPostIdAsync(int postId);
    Task <Comment> UpdateAsync(int commentId, Comment comment);
    Task <Comment> DeleteAsync(int commentId);
    
    Task<bool> AddCommentLikeAsync(int userId, int commentId);
}