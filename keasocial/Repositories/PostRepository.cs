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

    public async Task<List<Post>> GetAsync()
    {
        return await _keasocialDbContext.Posts.ToListAsync();
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
}