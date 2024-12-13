using System.Security.Claims;
using keasocial.Dto;
using keasocial.Models;
using keasocial.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("{uuid}")]
    public async Task<ActionResult<List<Post>>> Get(string uuid)
    {
        var post = await _postService.GetAsync(uuid);
        return Ok(post);
    }
    
    [HttpPost]
    public async Task<ActionResult<Post>> Create([FromBody] PostCreateDto postCreate)
    {
        var createdPost = await _postService.CreateAsync(postCreate);
        return CreatedAtAction(nameof(Get), new { id = createdPost.Uuid }, createdPost);
    }
    
    [Authorize]
    [HttpPut("{uuid}")]
    public async Task<ActionResult<Post>> Put(string uuid, [FromBody] PostUpdateDto postCreate)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        var updatedPost = await _postService.UpdateAsync(uuid, postCreate, userId);
        return Ok(updatedPost);
    }
    
    [Authorize]
    [HttpDelete("{postUuid}")]
    public async Task<ActionResult> Delete(string postUuid)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        
        await _postService.DeleteAsync(postUuid, userId);
        return Ok("Post deleted successfully.");
    }
    
    [Authorize]
    [HttpPost("like/{postUuid}")]
    public async Task<ActionResult> Post(string postUuid)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _postService.AddPostLikeAsync(postUuid, userId);
        
        return Ok("Post liked successfully.");
    }
}