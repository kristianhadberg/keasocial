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