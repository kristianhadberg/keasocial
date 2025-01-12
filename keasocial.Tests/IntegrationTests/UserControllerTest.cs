using System.Net;
using System.Net.Http.Json;
using keasocial.Dto;
using keasocial.Models;
using Xunit.Abstractions;

namespace keasocial.Tests.IntegrationTests;

[Trait("Category", "Integration")]
public class UserControllerTest : IntegrationTestSetup
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UserControllerTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Get_NonExistingId_ReturnsNoContent()
    {
        var nonExistingId = 99999;
        var getUserResponse = await TestClient.GetAsync($"api/User/{nonExistingId}");
        
        Assert.Equal(HttpStatusCode.NoContent, getUserResponse.StatusCode);
    }

    [Fact]
    public async Task Get_ValidId_ReturnsUser()
    {
        var newUser = new UserCreateDto
        {
            Name = "Valid Name",
            Email = "validemail@example.com",
            Password = "validpassword123"
        };

        // Create user
        var createUserResponse = await TestClient.PostAsJsonAsync("api/User/register", newUser);
        var userCreated = await createUserResponse.Content.ReadAsAsync<User>();   
    
        // Try to get user by ID from db
        var getUserResponse = await TestClient.GetAsync($"api/User/{userCreated.UserId}");
        var userReturned = await getUserResponse.Content.ReadAsAsync<User>();
        
        Assert.Equal(HttpStatusCode.OK, getUserResponse.StatusCode);
        Assert.Equal(newUser.Email, userReturned.Email);
        Assert.Equal(newUser.Name, userReturned.Name);
    }

    [Fact]
    public async Task Register_WithValidUser_SavesUserToDb()
    {
        var newUser = new UserCreateDto
        {
            Name = "Valid Name",
            Email = "validemail@example.com",
            Password = "validpassword123"
        };

        // Create user
        var createUserResponse = await TestClient.PostAsJsonAsync("api/User/register", newUser);
        var userCreated = await createUserResponse.Content.ReadAsAsync<User>();
        
        // Try to get user by ID from db
        var getUserResponse = await TestClient.GetAsync($"api/User/{userCreated.UserId}");
        var userReturned = await getUserResponse.Content.ReadAsAsync<User>();
        
        Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);
        Assert.Equal(userReturned.UserId, userCreated.UserId);
        Assert.Equal(userReturned.Name, userCreated.Name);
    }
    
    [Fact]
    public async Task Register_WithInvalidEmailUser_ReturnsException()
    {
        var newUser = new UserCreateDto
        {
            Name = "Valid Name",
            Email = "invalidemail",
            Password = "validpassword123"
        };

        // Create user
        var createUserResponse = await TestClient.PostAsJsonAsync("api/User/register", newUser);
        var errorResponse = await createUserResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        
        Assert.Equal(HttpStatusCode.BadRequest, createUserResponse.StatusCode);
        Assert.Equal("Invalid email address.", errorResponse["error"]);
    }

    [Fact]
    public async Task Login_WithInvalidUser_ReturnsException()
    {
        var loginRequest = new LoginRequestDto()
        {
            Email = "emailthatdoesntexistindb@google.com",
            Password = "randompassword123"
        };
        
        // Try to login
        var loginResponse = await TestClient.PostAsJsonAsync("api/User/login", loginRequest);
        var errorResponse = await loginResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        
        Assert.Equal(HttpStatusCode.BadRequest, loginResponse.StatusCode);
        Assert.Equal("Invalid username or password.", errorResponse["error"]);
    }

    [Fact]
    public async Task Login_ValidUser_ReturnsJwtToken()
    {
        var newUser = new UserCreateDto
        {
            Name = "Valid Name",
            Email = "validemail@example.com",
            Password = "validpassword123"
        };
        
        var loginRequest = new LoginRequestDto()
        {
            Email = newUser.Email,
            Password = newUser.Password
        };
        
        // Create user
        var createUserResponse = await TestClient.PostAsJsonAsync("api/User/register", newUser);
        
        var loginResponse = await TestClient.PostAsJsonAsync("api/User/login", loginRequest);
        var returnedUser = await loginResponse.Content.ReadAsAsync<LoginResponseDto>();
        
        Assert.Equal(HttpStatusCode.Created, createUserResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        Assert.NotNull(returnedUser.Token);
    }
}