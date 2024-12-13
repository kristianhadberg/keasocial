using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
using keasocial.Services.Interfaces;

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
    
    public async Task<Comment> GetAsync(string commentUuid, string postUuid)
    {
        var comment = await _commentRepository.GetAsync(commentUuid, postUuid);
        if (comment == null)
        {
            throw new ArgumentException("Comment not found or does not belong to the specified post.");
        }

        return comment;
    }
    
    public async Task<List<CommentDto>> GetAsync()
    {
        return await _commentRepository.GetAsync();
    }

    public async Task<IEnumerable<CommentDto>> GetByPostIdAsync(string postUuid)
    {
        // Ensure the post exists
        var post = await _postRepository.GetAsync(postUuid);
        if (post == null)
        {
            throw new ArgumentException($"Post with ID {postUuid} does not exist.");
        }

        // Fetch and return comments
        return await _commentRepository.GetByPostIdAsync(postUuid);
    }


    public async Task<CommentDto> CreateAsync(CommentCreateDto commentCreateDto, string postUuid)
    {
        if (string.IsNullOrWhiteSpace(commentCreateDto.Content) ||
            commentCreateDto.Content.Length < 5 ||
            commentCreateDto.Content.Length > 200)
        {
            throw new ArgumentException("Content must be between 5 and 200 characters.");
        }

        var post = await _postRepository.GetAsync(postUuid);
        if (post == null)
        {
            throw new ArgumentException($"Post with ID {postUuid} does not exist.");
        }

        var comment = new Comment
        {
            Content = commentCreateDto.Content,
            CreatedAt = DateTime.UtcNow,
            LikeCount = 0
        };

        return await _commentRepository.CreateAsync(comment, postUuid, commentCreateDto.UserUuid);
    }


    
    public async Task<Comment> UpdateAsync(CommentUpdateDto commentUpdateDto, string postUuid, string commentUuid, string userUuid)
    {
        if (string.IsNullOrWhiteSpace(commentUpdateDto.Content) || commentUpdateDto.Content.Length < 5 || commentUpdateDto.Content.Length > 200)
        {
            throw new ArgumentException("Content must be between 5 and 200 characters.");
        }

        Comment comment = await _commentRepository.GetAsync(commentUuid, postUuid);

        if (comment == null)
        {
            throw new ArgumentException("Comment not found.");
        }

        var isUserAuthorized = await _commentRepository.IsUserAuthorizedToChangeComment(userUuid, commentUuid);
        if (!isUserAuthorized)
        {
            throw new UnauthorizedAccessException("You can only update your own comments.");
        }

        comment.Content = commentUpdateDto.Content;
        comment.LikeCount = commentUpdateDto.LikeCount;
        var updatedComment = await _commentRepository.UpdateAsync(commentUuid, comment);
        return updatedComment;
    }
    
    public async Task<Comment> DeleteAsync(string commentUuid, string postUuid, string userUuid)
    {
        var comment = await _commentRepository.GetAsync(commentUuid, postUuid);

        if (comment == null)
        {
            throw new ArgumentException("Comment not found or does not belong to the specified post.");
        }

        var isUserAuthorized = await _commentRepository.IsUserAuthorizedToChangeComment(userUuid, commentUuid);
        if (!isUserAuthorized)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this post.");
        }

        return await _commentRepository.DeleteAsync(commentUuid);
    }
    

    public async Task<bool> AddCommentLikeAsync(string postUuid, string commentUuid, string userUuid)
    {
        var commentExists = await _commentRepository.GetAsync(commentUuid, postUuid);

        if (commentExists == null)
        {
            throw new KeyNotFoundException($"Comment with ID {commentUuid} not found.");
        }

        var success = await _commentRepository.AddCommentLikeAsync(commentUuid, userUuid);

        if (!success)
        {
            throw new BadHttpRequestException("You have already liked this comment.");
        }

        return true;
    }

}
