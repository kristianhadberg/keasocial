using keasocial.Data;
using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace keasocial.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly KeasocialDbContext _keasocialDbContext;

    public CommentRepository(KeasocialDbContext keasocialDbContext)
    {
        _keasocialDbContext = keasocialDbContext;
    }
    
    public async Task<Comment> GetAsync(string commentId)
    {
       return await _keasocialDbContext.Comments.FindAsync(commentId);
           
    }


    public async Task<List<CommentDto>> GetAsync()
    {
        return await _keasocialDbContext.Comments
            .Select(c => new CommentDto
            {
                CommentId = c.CommentId,
                PostId = c.PostId,
                UserId = c.UserId,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                LikeCount = c.LikeCount
            })
            .ToListAsync();
    }

    
    public async Task<IEnumerable<CommentDto>> GetByPostIdAsync(int postId)
    {
        return await _keasocialDbContext.Comments
            .Where(c => c.PostId == postId)
            .Select(c => new CommentDto
            {
                CommentId = c.CommentId,
                UserId = c.UserId,
                PostId = c.PostId,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                LikeCount = c.LikeCount,
                CommentLikes = c.CommentLikes.Select(cl => new CommentLikeDto
                {
                    UserId = cl.UserId,
                    CommentId = cl.CommentId
                }).ToList()
            })
            .ToListAsync();
    }




    public async Task<CommentDto> CreateAsync(Comment comment)
    {
        await _keasocialDbContext.Comments.AddAsync(comment);
        await _keasocialDbContext.SaveChangesAsync();

        var commentDto = new CommentDto
        {
            CommentId = comment.CommentId,
            UserId = comment.UserId,
            PostId = comment.PostId,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            LikeCount = comment.LikeCount,
        };

        return commentDto;
    }

    public async Task<Comment> UpdateAsync(string commentId, Comment comment)
    {
        var existingComment =  _keasocialDbContext.Comments.Update(comment);
        await _keasocialDbContext.SaveChangesAsync();
        return existingComment.Entity;
    }

    public async Task<Comment> DeleteAsync(string commentId)
    {
        var comment = await _keasocialDbContext.Comments.FindAsync(commentId);
        _keasocialDbContext.Comments.Remove(comment);
        await _keasocialDbContext.SaveChangesAsync();
        return comment; 
    }

    public async Task<bool> AddCommentLikeAsync(int userId, string commentId, int postId)
    {
        var existingLike = await _keasocialDbContext.CommentLikes
            .FirstOrDefaultAsync(cl => cl.UserId == userId && cl.CommentId == commentId);

        if (existingLike != null)
        {
            return false; 
        }

        var commentLike = new CommentLike
        {
            UserId = userId,
            CommentId = commentId
        };

        await _keasocialDbContext.CommentLikes.AddAsync(commentLike);
        await _keasocialDbContext.SaveChangesAsync();

        return true;
    }

    
}
