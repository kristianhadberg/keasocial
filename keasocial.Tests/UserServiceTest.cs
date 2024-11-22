using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
using keasocial.Security;
using keasocial.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace keasocial.Tests;

public class UserServiceTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;

    public UserServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();

        // Mock the configuration settings for the JwtService
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c["Jwt:Key"]).Returns("YourTestSecretKey");
        configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("YourTestIssuer");
        var jwtService = new JwtService(configurationMock.Object);
        
        _userService = new UserService(_userRepositoryMock.Object, jwtService);
    }
    
    /*
     *  Maybe we can use this old test further down the line
     *  as inspiration for integration tests 
     */
    
    /*[Theory]
    [InlineData("J")]
    [InlineData("Jo")]
    [InlineData("John Geronimo Doe Johnson")]
    [InlineData("JimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmyJimm")] // 49 char name
    [InlineData("JimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmy")] // 50 char name
    public async void Test_CreateUser_Name_ShouldBe_SpecificLength(string name)
    {
        var userCreateDto = new UserCreateDto
        {
            Name = name,
            Email = "john@example.com",
            Password = "testpassword"
        };

        var createdUser = new User
        {
            UserId = 1,
            Name = userCreateDto.Name,
            Email = userCreateDto.Email,
            Password = userCreateDto.Password
        };
        
        _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(userCreateDto.Email))
            .ReturnsAsync((User)null);
        _userRepositoryMock.Setup(repo => repo.Create(It.IsAny<User>())).ReturnsAsync(createdUser);
        
        var result = await _userService.Create(userCreateDto);
        Assert.Equal(createdUser.Name, result.Name);
        
        // Verifies that the mock repository is actually used
        _userRepositoryMock.Verify(repo => repo.GetByEmailAsync(It.IsAny<string>()), Times.Once);
    }*/
    
    [Theory]
    [InlineData("J")]
    [InlineData("Jo")]
    [InlineData("John Geronimo Doe Johnson")]
    [InlineData("JimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmyJimm")] // 49 char name
    [InlineData("JimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmy")] // 50 char name
    [InlineData("JimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmyyy")] // 50 char name
    public void Test_CreateUser_Name_ShouldBe_SpecificLength(string name)
    {
        var userCreateDto = new UserCreateDto
        {
            Name = name,
            Email = "john@example.com",
            Password = "testpassword"
        };

        var exception = Record.Exception(() => _userService.ValidateUserCreateDto(userCreateDto));
        
        // Assert that no exception is thrown and therefore validation is valid.
        Assert.Null(exception);
    }
}