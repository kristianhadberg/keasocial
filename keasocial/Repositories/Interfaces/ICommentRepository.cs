using keasocial.Models;
using keasocial.Dto;

namespace keasocial.Repositories.Interfaces;

public interface ICommentRepository
{
    Task <CommentDto> CreateAsync(Comment comment, string postUuid, string userUuid);
    Task <Comment> GetAsync(string commentUuid, string postUuid);
    Task <List<CommentDto>> GetAsync();
    Task <IEnumerable<CommentDto>> GetByPostIdAsync(string postUuid);
    Task <Comment> UpdateAsync(string uuid, Comment comment);
    Task <Comment> DeleteAsync(string uuid);
    Task<bool> AddCommentLikeAsync(string commentUuid, string userUuid);
    Task<bool> IsUserAuthorizedToChangeComment(string userUuid, string commentUuid);


}