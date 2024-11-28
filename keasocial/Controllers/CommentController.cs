using System.Security.Claims;
using keasocial.Dto;
using keasocial.Models;
using keasocial.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace keasocial.Controllers;


[ApiController]
[Route("api/{postId}/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly IUserService _userService;
    private readonly IPostService _postService;

    public CommentController(ICommentService commentService, IUserService userService, IPostService postService)
    {
        _commentService = commentService;
        _userService = userService;
        _postService = postService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Comment>>> Get(int postId)
    {
        var comments = await _commentService.GetByPostIdAsync(postId);
        return Ok(comments);
    }

    [HttpGet("{commentId}")]
    public async Task<ActionResult<Comment>> Get(int postId, int commentId)
    {
        var comment = await _commentService.GetAsync(commentId, postId);
        if (comment == null || comment.PostId != postId)
        {
            return NotFound();
        }
        return Ok(comment);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Comment>> Create(int postId, [FromBody] CommentCreateDto commentCreate)
    {

        var createdComment = await _commentService.CreateAsync(commentCreate, postId);
        return CreatedAtAction(nameof(Get), new { postId, commentId = createdComment.CommentId }, createdComment);
    }

    [Authorize]
    [HttpPut("{commentId}")]
    public async Task<ActionResult<Comment>> Put(int postId, int commentId, [FromBody] CommentUpdateDto commentUpdate)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var updatedComment = await _commentService.UpdateAsync(commentId, commentUpdate, userId);
        return Ok(updatedComment);
    }

    [Authorize]
    [HttpDelete("{commentId}")]
    public async Task<ActionResult> Delete(int postId, int commentId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        await _commentService.DeleteAsync(userId, commentId);
        return Ok("Comment deleted successfully.");
    }

    [Authorize]
    [HttpPost("{commentId}/like")]
    public async Task<ActionResult> Like(int postId, int commentId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        await _commentService.AddCommentLikeAsync(userId, commentId, postId);
        return Ok("Comment liked successfully.");
    }
}
