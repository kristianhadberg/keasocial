using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
using MongoDB.Driver;
using MongoDB.Bson;

namespace keasocial.Repositories;

public class MongoPostRepository : IPostRepository
{
    private readonly IMongoCollection<Post> _posts;
    private readonly IMongoCollection<PostLike> _postLikes;

    public MongoPostRepository(IMongoDatabase database)
    {
        _posts = database.GetCollection<Post>("Posts");
        _postLikes = database.GetCollection<PostLike>("PostLikes");
    }

    public async Task<Post> GetAsync(int id)
    {
        return await _posts.Find(p => p.PostId == id).FirstOrDefaultAsync();
    }

    public async Task<List<PostDto>> GetAsync()
    {
        var posts = await _posts.Find(_ => true).ToListAsync();

        var postDtos = posts.Select(post => new PostDto
        {
            PostId = post.PostId,
            UserId = post.UserId,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            LikeCount = post.LikeCount,
            Comments = post.Comments?.Select(CommentToDto).ToList() ?? new List<CommentDto>()
        }).ToList();

        return postDtos;
    }

    public async Task<Post> CreateAsync(Post post)
    {
        await _posts.InsertOneAsync(post);
        return post;
    }

    public async Task<Post> UpdateAsync(int id, Post post)
    {
        var result = await _posts.ReplaceOneAsync(p => p.PostId == id, post);
        if (result.MatchedCount == 0)
        {
            throw new KeyNotFoundException($"Post with ID {id} not found.");
        }
        return post;
    }

    public async Task<Post> DeleteAsync(int id)
    {
        var result = await _posts.FindOneAndDeleteAsync(p => p.PostId == id);
        if (result == null)
        {
            throw new KeyNotFoundException($"Post with ID {id} not found.");
        }
        return result;
    }

    public async Task<bool> AddPostLikeAsync(int userId, int postId)
    {
        var existingLike = await _postLikes.Find(pl => pl.UserId == userId && pl.PostId == postId).FirstOrDefaultAsync();
        if (existingLike != null)
        {
            return false;
        }

        var postLike = new PostLike
        {
            UserId = userId,
            PostId = postId
        };

        await _postLikes.InsertOneAsync(postLike);

        // Increment the LikeCount in the post document
        var updateResult = await _posts.UpdateOneAsync(
            p => p.PostId == postId,
            Builders<Post>.Update.Inc(p => p.LikeCount, 1)
        );

        return updateResult.ModifiedCount > 0;
    }

    public async Task<List<PostLikeView>> GetPostLikesAsync(int postId)
    {
        var postLikes = await _postLikes.Find(pl => pl.PostId == postId).ToListAsync();

        return postLikes.Select(pl => new PostLikeView
        {
            PostId = pl.PostId,
            UserId = pl.UserId
        }).ToList();
    }

    private CommentDto CommentToDto(Comment comment)
    {
        return new CommentDto
        {
            CommentId = comment.CommentId,
            UserId = comment.UserId,
            PostId = comment.PostId,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            LikeCount = comment.LikeCount,
            CommentLikes = comment.CommentLikes?.Select(CommentLikeToDto).ToList() ?? new List<CommentLikeDto>()
        };
    }

    private CommentLikeDto CommentLikeToDto(CommentLike commentLike)
    {
        return new CommentLikeDto
        {
            CommentLikeId = commentLike.CommentLikeId,
            UserId = commentLike.UserId,
            CommentId = commentLike.CommentId
        };
    }
}
