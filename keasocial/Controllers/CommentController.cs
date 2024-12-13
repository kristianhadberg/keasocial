using System.Security.Claims;
using keasocial.Dto;
using keasocial.Models;
using keasocial.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace keasocial.Controllers;


[ApiController]
[Route("api/{postUuid}/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    [Route("~/api/[controller]")]
    public async Task<ActionResult<List<CommentDto>>> Get()
    {
        var comments = await _commentService.GetAsync();
        return Ok(comments);
    }

    [HttpGet]
    public async Task<ActionResult<List<CommentDto>>> GetByPostUuid(string postUuid)
    {
        var comments = await _commentService.GetByPostIdAsync(postUuid);
        return Ok(comments);
    }
    
    [HttpGet("{commentUuid}")]
    public async Task<ActionResult<Comment>> Get(string postUuid, string commentUuid)
    {
        var comment = await _commentService.GetAsync(commentUuid, postUuid);
        if (comment == null)
        {
            return NotFound();
        }
        return Ok(comment);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create(string postUuid, [FromBody] CommentCreateDto commentCreate)
    {
        var createdComment = await _commentService.CreateAsync(commentCreate, postUuid);
        return Ok(createdComment);
    }

    [Authorize]
    [HttpPut("{commentUuid}")]
    public async Task<ActionResult<CommentUpdateDto>> Put(string postUuid, string commentUuid, [FromBody] CommentUpdateDto commentUpdate)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var updatedComment = await _commentService.UpdateAsync(commentUpdate, postUuid, commentUuid, userId);
        return Ok(updatedComment);
    }

    [Authorize]
    [HttpDelete("{commentUuid}")]
    public async Task<ActionResult> Delete(string postUuid, string commentUuid)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        
        await _commentService.DeleteAsync(commentUuid, postUuid, userId);
        return Ok("Comment deleted successfully.");
    }

    [Authorize]
    [HttpPost("{commentUuid}/like")]
    public async Task<ActionResult> Like(string postUuid, string commentUuid)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _commentService.AddCommentLikeAsync(postUuid, commentUuid, userId);
        return Ok("Comment liked successfully.");
    }
}
