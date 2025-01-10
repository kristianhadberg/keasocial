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
    private readonly IUserService _userService;

    public PostController(IPostService postService, IUserService userService)
    {
        _postService = postService;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<PostDto>> Get()
    {
        var posts = await _postService.GetAsync();
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> Get(int id)
    {
        var post = await _postService.GetAsync(id);
        return Ok(post);
    }
    
    [HttpPost]
    public async Task<ActionResult<Post>> Create([FromBody] PostCreateDto postCreate)
    {
        var createdPost = await _postService.CreateAsync(postCreate);
        return CreatedAtAction(nameof(Get), new { id = createdPost.PostId }, createdPost);
    }
    
    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<Post>> Put(int id, [FromBody] PostUpdateDto postCreate)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var updatedPost = await _postService.UpdateAsync(id, postCreate, userId);
        return Ok(updatedPost);
    }
    
    [Authorize]
    [HttpDelete("{postId}")]
    public async Task<ActionResult> Delete(int postId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        
        await _postService.DeleteAsync(userId, postId);
        return Ok("Post deleted successfully.");
    }

    [HttpGet("like/{postId}")]
    public async Task<ActionResult<List<PostLikeView>>> GetPostLikes(int postId)
    {
        var postLikeView = await _postService.GetPostLikesAsync(postId);
        return Ok(postLikeView);
    }

    [HttpGet("most-liked")]
    public async Task<ActionResult<List<PostDto>>> GetMostLikedPosts()
    {
        var mostLikedPosts = await _postService.GetMostLikedPostsAsync();
        return Ok(mostLikedPosts);
    }

    [Authorize]
    [HttpPost("like/{postId}")]
    public async Task<ActionResult> Post(int postId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        await _postService.AddPostLikeAsync(userId, postId);
        
        return Ok("Post liked successfully.");
    }
}