using keasocial.Dto;
using keasocial.Repositories.Interfaces;
using keasocial.Security;
using keasocial.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace keasocial.Tests.UnitTests;

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

    
    /*
     * Not sure if these tests are necessary, since its essentially testing
     * built-in language functionality to validate emails?
     */
    [Theory]
    [InlineData("test@test.com")]
    [InlineData("test@test.dk")]
    [InlineData("newuser@gmail.com")]
    public void ValidateUserCreateDto_ValidEmail_ThrowsNoException(string email)
    {
        var userCreateDto = new UserCreateDto
        {
            Name = "Jimmy Doe",
            Email = email,
            Password = "testpassword"
        };

        var exception = Record.Exception(() => _userService.ValidateUserCreateDto(userCreateDto));
        
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("new@")]
    [InlineData("myemail.com")]
    [InlineData("myemail")]
    public void ValidateUserCreateDto_InvalidEmail_ThrowsException(string email)
    {
        var userCreateDto = new UserCreateDto
        {
            Name = "Jimmy Doe",
            Email = email,
            Password = "testpassword"
        };

        var exception = Record.Exception(() => _userService.ValidateUserCreateDto(userCreateDto));
        
        Assert.IsType<ArgumentException>(exception);
    }
}