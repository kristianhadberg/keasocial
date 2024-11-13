using keasocial.Dto;
using keasocial.Models;
using keasocial.Services.Interfaces;
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
    
    [HttpPost]
    public async Task<ActionResult<Post>> Create([FromBody] PostCreateDto postCreate)
    {
        var createdPost = await _postService.CreateAsync(postCreate);
        return CreatedAtAction(nameof(Get), new { id = createdPost.PostId }, createdPost);
        return Ok(createdPost);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<Post>> Put(int id, [FromBody] PostUpdateDto postCreate)
    {
        var updatedPost = await _postService.UpdateAsync(id, postCreate);
        return Ok(updatedPost);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _postService.DeleteAsync(id);
        return Ok();
    }
}