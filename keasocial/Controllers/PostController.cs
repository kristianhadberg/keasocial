using keasocial.Models;
using keasocial.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace keasocial.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public async Task<ActionResult<Post>> Get()
    {
        var posts = await _postService.GetAsync();
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<List<Post>>> Get(int id)
    {
        var post = await _postService.GetAsync(id);
        return Ok(post);
    } 
}