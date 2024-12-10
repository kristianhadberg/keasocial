using keasocial.Data;
using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace keasocial.Repositories;

public class PostRepository : IPostRepository
{
    private readonly KeasocialDbContext _keasocialDbContext;

    public PostRepository(KeasocialDbContext keasocialDbContext)
    {
        _keasocialDbContext = keasocialDbContext;
    }

    public async Task<Post> GetAsync(int id)
    {
        return await _keasocialDbContext.Posts.FindAsync(id);
    }

    public async Task<List<PostDto>> GetAsync()
    {
        var posts = await _keasocialDbContext.Posts
            .Include(p => p.Comments)
                .ThenInclude(c => c.CommentLikes)
            .ToListAsync();

        var postDtos = posts.Select(post => new PostDto
        {
            PostId = post.PostId,
            UserId = post.UserId,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            LikeCount = post.LikeCount,
            Comments = post.Comments.Select(CommentToDto).ToList()
        }).ToList();

        return postDtos;
    }
    
    public async Task<Post> CreateAsync(Post post)
    {
        await _keasocialDbContext.Posts.AddAsync(post);
        await _keasocialDbContext.SaveChangesAsync();
        
        return post;
    }
    
    public async Task<Post> UpdateAsync(int id, Post post)
    {
        var updatedPost = _keasocialDbContext.Posts.Update(post);
        await _keasocialDbContext.SaveChangesAsync();
        return updatedPost.Entity;
    }
    
    public async Task<Post> DeleteAsync(int id)
    {
        var post = await _keasocialDbContext.Posts.FindAsync(id);
        _keasocialDbContext.Posts.Remove(post);
        await _keasocialDbContext.SaveChangesAsync();
        return post;
    }

    public async Task<bool> AddPostLikeAsync(int userId, int postId)
    {
        var existingLike =
            await _keasocialDbContext.PostLikes.FirstOrDefaultAsync(pl => pl.UserId == userId && pl.PostId == postId);

        if (existingLike != null)
        {
            return false;
        }

        var postLike = new PostLike
        {
            UserId = userId,
            PostId = postId
        };

        await _keasocialDbContext.PostLikes.AddAsync(postLike);
        await _keasocialDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<List<PostLikeView>> GetPostLikesAsync(int postId)
    {
        var postLikes = await _keasocialDbContext.PostLikeViews
            .Where(pl => pl.PostId == postId)
            .ToListAsync();
        
        return postLikes;
    }

    public async Task<List<PostDto>> GetMostLikedPostsAsync()
    {
        // Fetch posts using stored procedure
        var posts = await _keasocialDbContext.Posts
            .FromSqlInterpolated($"CALL GetMostLikedPosts()")
            .ToListAsync();

        // Cant use Include/ThenInclude directly on the result of the stored procedure
        // So instead we fetch the comments independently
        var postIds = posts.Select(p => p.PostId).ToList();
        var comments = await _keasocialDbContext.Comments
            .Where(c => postIds.Contains(c.PostId))
            .Include(c => c.CommentLikes) // Include CommentLikes
            .ToListAsync();

        var mostLikedPostDtos = posts.Select(post => new PostDto
        {
            PostId = post.PostId,
            UserId = post.UserId,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            LikeCount = post.LikeCount,
            Comments = comments
                .Where(c => c.PostId == post.PostId)
                .Select(CommentToDto)
                .ToList()
        }).ToList();

        return mostLikedPostDtos;
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
            CommentLikes = comment.CommentLikes.Select(CommentLikeToDto).ToList()
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