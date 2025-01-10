using System.Net;
using System.Net.Http.Json;
using keasocial.Dto;
using keasocial.Models;
using Xunit.Abstractions;

namespace keasocial.Tests.IntegrationTests;

public class PostControllerTest : IntegrationTestSetup
{
    private readonly ITestOutputHelper _testOutputHelper;

    public PostControllerTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void LogEnvironment()
    {
        var currentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        _testOutputHelper.WriteLine($"Current Environment: {currentEnvironment ?? "Not Set"}");
    }
    
    /*[Fact]
    public async Task Get_WithoutAnyPosts_ReturnsEmptyList()
    {
        var response = await TestClient.GetAsync("api/Post");
        var rawResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response Content: {rawResponse}");
        
        /*var posts = await response.Content.ReadFromJsonAsync<List<PostDto>>();#1#
        var posts = await response.Content.ReadAsAsync<List<Post>>();

        Assert.NotNull(response);
        Assert.Empty(posts);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }*/

    /*[Fact]
    public async Task Get_WithPostsInDb_ReturnsListOfPosts()
    {
        var newPost = new PostCreateDto
        {
            UserId = 1,
            Content = "Valid content",
            LikeCount = 0,
        };
        await CreatePostAsync(newPost);
        
        var response = await TestClient.GetAsync("api/Post/1");
        
        var rawResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response Content: {rawResponse}");
        
        var returnedPost = await response.Content.ReadAsAsync<Post>();
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(newPost.Content, returnedPost.Content);
    }*/
}