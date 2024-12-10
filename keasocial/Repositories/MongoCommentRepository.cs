using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
using MongoDB.Driver;
using MongoDB.Bson;

namespace keasocial.Repositories;

public class MongoCommentRepository : ICommentRepository
{
    private readonly IMongoCollection<Comment> _comments;
    private readonly IMongoCollection<CommentLike> _commentLikes;

    public MongoCommentRepository(IMongoDatabase database)
    {
        _comments = database.GetCollection<Comment>("Comments");
        _commentLikes = database.GetCollection<CommentLike>("CommentLikes");
    }

    public async Task<Comment> GetAsync(int commentId)
    {
        return await _comments.Find(c => c.CommentId == commentId).FirstOrDefaultAsync();
    }

    public async Task<List<CommentDto>> GetAsync()
    {
        var comments = await _comments.Find(_ => true).ToListAsync();

        return comments.Select(c => new CommentDto
        {
            CommentId = c.CommentId,
            PostId = c.PostId,
            UserId = c.UserId,
            Content = c.Content,
            CreatedAt = c.CreatedAt,
            LikeCount = c.LikeCount
        }).ToList();
    }

    public async Task<IEnumerable<CommentDto>> GetByPostIdAsync(int postId)
    {
        var comments = await _comments.Find(c => c.PostId == postId).ToListAsync();

        return comments.Select(c => new CommentDto
        {
            CommentId = c.CommentId,
            UserId = c.UserId,
            PostId = c.PostId,
            Content = c.Content,
            CreatedAt = c.CreatedAt,
            LikeCount = c.LikeCount,
            CommentLikes = c.CommentLikes?.Select(cl => new CommentLikeDto
            {
                UserId = cl.UserId,
                CommentId = cl.CommentId
            }).ToList() ?? new List<CommentLikeDto>()
        });
    }

    public async Task<CommentDto> CreateAsync(Comment comment)
    {
        await _comments.InsertOneAsync(comment);

        return new CommentDto
        {
            CommentId = comment.CommentId,
            UserId = comment.UserId,
            PostId = comment.PostId,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            LikeCount = comment.LikeCount
        };
    }

    public async Task<Comment> UpdateAsync(int commentId, Comment comment)
    {
        var result = await _comments.ReplaceOneAsync(c => c.CommentId == commentId, comment);
        if (result.MatchedCount == 0)
        {
            throw new KeyNotFoundException($"Comment with ID {commentId} not found.");
        }
        return comment;
    }

    public async Task<Comment> DeleteAsync(int commentId)
    {
        var result = await _comments.FindOneAndDeleteAsync(c => c.CommentId == commentId);
        if (result == null)
        {
            throw new KeyNotFoundException($"Comment with ID {commentId} not found.");
        }
        return result;
    }

    public async Task<bool> AddCommentLikeAsync(int userId, int commentId, int postId)
    {
        var existingLike = await _commentLikes.Find(cl => cl.UserId == userId && cl.CommentId == commentId).FirstOrDefaultAsync();

        if (existingLike != null)
        {
            return false;
        }

        var commentLike = new CommentLike
        {
            UserId = userId,
            CommentId = commentId
        };

        await _commentLikes.InsertOneAsync(commentLike);

        // Increment the LikeCount in the comment document
        var updateResult = await _comments.UpdateOneAsync(
            c => c.CommentId == commentId,
            Builders<Comment>.Update.Inc(c => c.LikeCount, 1)
        );

        return updateResult.ModifiedCount > 0;
    }
}
