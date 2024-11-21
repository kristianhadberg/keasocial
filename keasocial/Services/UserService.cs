using System.ComponentModel.DataAnnotations;
using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
using keasocial.Security;
using keasocial.Services.Interfaces;

namespace keasocial.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtService _jwtService;

    public UserService(IUserRepository userRepository, JwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<User> GetAsync(int id)
    {
        return await _userRepository.GetAsync(id);
    }

    public async Task<List<User>> GetAsync()
    {
        return await _userRepository.GetAsync();
    }

    public async Task<User> Create(UserCreateDto userCreateDto)
    {
        if (string.IsNullOrWhiteSpace(userCreateDto.Name) || userCreateDto.Name.Length is < 1 or > 50)
        {
            throw new ArgumentException("Name must be between 1 and 50 characters.");
        }

        if (!IsValid(userCreateDto.Email))
        {
            throw new ArgumentException("Invalid email address.");
        }
        
        var existingUser = await _userRepository.GetByEmailAsync(userCreateDto.Email);
        if (existingUser != null)
        {
            throw new ArgumentException("A user with this email already exists.");
        }

        if (userCreateDto.Password.Length is < 5 or > 20)
        {
            throw new ArgumentException("Password must be between 5 and 20 characters.");
        }
        
        var user = new User
        {
            Name = userCreateDto.Name,
            Email = userCreateDto.Email,
            Password = userCreateDto.Password
        };
        
        return await _userRepository.Create(user);
    }

    /*
     * Login is very simple at the moment (non-existent)
     * JWT needs to be added
     */
    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var user = await _userRepository.Login(loginRequestDto);

        if (user == null || user.Password != loginRequestDto.Password)
        {
            throw new ArgumentException("Invalid username or password.");
        }

        var token = _jwtService.GenerateToken(user.Email, user.UserId);

        var loginResponse = new LoginResponseDto
        {
            Name = user.Name,
            Email = user.Email,
            Token = token
        };
        
        return loginResponse;
    }

    private bool IsValid(string email)
    {
        return new EmailAddressAttribute().IsValid(email);
    }
}