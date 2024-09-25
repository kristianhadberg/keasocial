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
    public async Task<List<User>> Get()
    {
        return await _userService.GetAsync();
    }
    
    [HttpGet("${userId}")]
    public async Task<User> Get(int userId)
    {
        return await _userService.GetAsync(userId);
    }
    
    
}