using System.Security.Claims;
using keasocial.Dto;
using keasocial.Models;
using keasocial.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace keasocial.Controllers;


[ApiController]
[Route("api/[controller]")]

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
    public async Task<ActionResult<Comment>> Get()
    {
        var comments = await _commentService.GetAsync();
        return Ok(comments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<List<Comment>>> Get(int id)
    {
        var comment = await _commentService.GetAsync(id);
        return Ok(comment);
    }

    [HttpPost]
    public async Task<ActionResult<Comment>> Create([FromBody] CommentCreateDto commentCreate)
    {
        var createdComment = await _commentService.CreateAsync(commentCreate);
        return CreatedAtAction(nameof(Get), new { id = createdComment.CommentId }, createdComment);
        return Ok(createdComment);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<Comment>> Put(int id, [FromBody] CommentUpdateDto commentUpdate)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var updatedComment = await _commentService.UpdateAsync(id, commentUpdate, userId);
        return Ok(updatedComment);
    }

    [Authorize]
    [HttpDelete("{commentId}")]
    public async Task<ActionResult> Delete(int commentId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        await _commentService.DeleteAsync(userId, commentId);
        return Ok();
    }

    [Authorize]
    [HttpPost("like/{commentId}")]
    public async Task<ActionResult> Post(int commentId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        await _commentService.AddCommentLikeAsync(userId, commentId);
        return Ok();
    }




}
