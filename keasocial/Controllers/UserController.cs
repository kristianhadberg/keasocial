using keasocial.Dto;
using keasocial.Models;
using keasocial.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace keasocial.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<User>> Get()
    {
        var users = await _userService.GetAsync();
        return Ok(users);
    }
    
    [HttpGet("${userId}")]
    public async Task<ActionResult<List<User>>> Get(int userId)
    {
        var user = await _userService.GetAsync(userId);
        return Ok(user);
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Post([FromBody]UserCreateDto userCreateDto)
    {
        var newUser = await _userService.Create(userCreateDto);
        return CreatedAtAction(nameof(Get), new { id = newUser.UserId }, newUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Post([FromBody] LoginRequestDto loginRequestDto)
    {
        var user = await _userService.Login(loginRequestDto);
        return Ok(user);
    }
    
}