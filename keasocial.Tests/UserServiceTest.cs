using keasocial.Dto;
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
    public void ValidateUserCreateDto_ValidNameLength_ThrowsNoException(string name)
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

    [Theory]
    [InlineData("")]
    [InlineData("JimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmyy")] // 51 char name
    [InlineData("JimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmyJimmyyy")] // 52 char name
    public void ValidateUserCreateDto_TooLongName_ThrowsException(string name)
    {
        var userCreateDto = new UserCreateDto
        {
            Name = name,
            Email = "john@example.com",
            Password = "testpassword"
        };

        var exception = Record.Exception(() => _userService.ValidateUserCreateDto((userCreateDto)));

        Assert.IsType<ArgumentException>(exception);
    }

    [Theory]
    [InlineData("fivec")]
    [InlineData("sixsix")]
    [InlineData("verystrongpw")]
    [InlineData("nineteencharacterpw")] // 19 char pw
    [InlineData("twentycharacterpw123")] // 20 char pw
    public void ValidateUserCreateDto_ValidPasswordLength_ThrowsNoException(string password)
    {
        var userCreateDto = new UserCreateDto
        {
            Name = "Jimmy Doe",
            Email = "john@example.com",
            Password = password
        };

        var exception = Record.Exception(() => _userService.ValidateUserCreateDto(userCreateDto));
        
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("four")]
    [InlineData("twenty1characterpw123")] // 21 char pw
    [InlineData("twenty2characterpw1234")] // 22 char pw
    public void ValidateUserCreateDto_InvalidPasswordLength_ThrowsArgumentException(string password)
    {
        var userCreateDto = new UserCreateDto
        {
            Name = "Jimmy Doe",
            Email = "john@example.com",
            Password = password
        };

        var exception = Record.Exception(() => _userService.ValidateUserCreateDto(userCreateDto));
        
        Assert.IsType<ArgumentException>(exception);
    }
}