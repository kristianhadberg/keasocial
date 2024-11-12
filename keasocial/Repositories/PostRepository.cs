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
}