using System.Net;
using System.Net.Http.Json;
using keasocial.Dto;
using keasocial.Models;

namespace keasocial.Tests.IntegrationTests;

public class PostControllerTest : IntegrationTestSetup
{
    [Fact]
    public async Task Get_WithoutAnyPosts_ReturnsEmptyList()
    {
        var response = await TestClient.GetAsync("api/Post");
        Console.WriteLine(response.Content);
        Console.WriteLine(response.StatusCode);
        var rawResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response Content: {rawResponse}");
        
        /*var posts = await response.Content.ReadFromJsonAsync<List<PostDto>>();*/
        var posts = await response.Content.ReadAsAsync<List<PostDto>>();

        Assert.NotNull(response);
        Assert.Empty(posts);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
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
        
        Console.WriteLine($"response: ${response.StatusCode}");
        
        var rawResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response Content: {rawResponse}");
        
        var returnedPost = await response.Content.ReadAsAsync<Post>();
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(newPost.Content, returnedPost.Content);
    }
}