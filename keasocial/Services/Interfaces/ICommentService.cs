using keasocial.Dto;
using keasocial.Models;

namespace keasocial.Services.Interfaces;


public interface ICommentService
{
    Task<CommentDto> CreateAsync(CommentCreateDto comment, string postUuid);
    Task<Comment> GetAsync(string commentUuid, string postUuid);
    Task<List<CommentDto>> GetAsync();
    Task<IEnumerable<CommentDto>> GetByPostIdAsync(string postId);
    Task<Comment> UpdateAsync(CommentUpdateDto comment, string postUuid, string commentUuid, string userUuid);
    Task<Comment> DeleteAsync(string commentUuiud, string postUuid, string userUuid);
    Task<bool> AddCommentLikeAsync(string postUuid, string commentUuid, string userUuid);
}