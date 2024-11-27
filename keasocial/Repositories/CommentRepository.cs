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
    
    public async Task<Comment> GetAsync(int commentId)
    {
       return await _keasocialDbContext.Comments.FindAsync(commentId);
           
    }

    public Task<List<CommentDto>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Comment>> GetByPostIdAsync(int postId)
    {
        throw new NotImplementedException();
    }


    public async Task<Comment> CreateAsync(Comment comment)
    {
        await _keasocialDbContext.Comments.AddAsync(comment);
        await _keasocialDbContext.SaveChangesAsync();
        
        return comment;
    }

    public async Task<Comment> UpdateAsync(int commentId, Comment comment)
    {
        var existingComment =  _keasocialDbContext.Comments.Update(comment);
        await _keasocialDbContext.SaveChangesAsync();
        return existingComment.Entity;
    }

    public async Task<Comment> DeleteAsync(int commentId)
    {
        var comment = await _keasocialDbContext.Comments.FindAsync(commentId);
        _keasocialDbContext.Comments.Remove(comment);
        await _keasocialDbContext.SaveChangesAsync();
        return comment; 
    }

    public async Task<bool> AddCommentLikeAsync(int userId, int commentId)
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
