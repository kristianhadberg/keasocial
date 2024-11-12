using keasocial.Models;
using keasocial.Repositories.Interfaces;
using keasocial.Services.Interfaces;

namespace keasocial.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;

    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<Post> GetAsync(int id)
    {
        return await _postRepository.GetAsync(id);
    }

    public async Task<List<Post>> GetAsync()
    {
        return await _postRepository.GetAsync();
    }
}