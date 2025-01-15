using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using keasocial.Dto;
using keasocial.Models;

namespace keasocial.Tests.IntegrationTests;

[Trait("Category", "Integration")]
// dotnet test --filter "Category=Integration" to run all integration tests
public class PostControllerTest : IntegrationTestSetup
{
    [Fact]
    public async Task Get_WithoutAnyPosts_ReturnsEmptyList()
    {
        var response = await TestClient.GetAsync("api/Post");

        var posts = await response.Content.ReadFromJsonAsync<List<PostDto>>();

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
        var returnedPost = await response.Content.ReadAsAsync<Post>();
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(newPost.Content, returnedPost.Content);
    }

    [Fact]
    public async Task Create_Post_SavesPostToDb()
    {
        var newPost = new PostCreateDto
        {
            UserId = 1,
            Content = "Post Creation Test",
            LikeCount = 0,
        };

        var createPostResponse = await TestClient.PostAsJsonAsync("api/Post", newPost);
        var createdPost = await createPostResponse.Content.ReadAsAsync<Post>();

        // Try to retrieve the newly added post by id
        var getCreatedPostResponse = await TestClient.GetAsync($"api/Post/{createdPost.PostId}");
        var returnedPost = await getCreatedPostResponse.Content.ReadAsAsync<Post>();
        
        Assert.Equal(HttpStatusCode.Created, createPostResponse.StatusCode); // assert the inital post creation status code
        Assert.Equal(createdPost.PostId, returnedPost.PostId);
        Assert.Equal(createdPost.Content, returnedPost.Content);
    }

    [Fact]
    public async Task Create_InvalidPost_ReturnsBadRequest()
    {
        var newPost = new PostCreateDto
        {
            UserId = 1,
            Content = "",
            LikeCount = 0,
        };
        
        var createPostResponse = await TestClient.PostAsJsonAsync("api/Post", newPost);
        
        Assert.Equal(HttpStatusCode.BadRequest, createPostResponse.StatusCode);
    }

    [Fact]
    public async Task DeletePost_UnauthorizedUser_ReturnsUnauthorized()
    {
        var newPost = new PostCreateDto
        {
            UserId = 1,
            Content = "Valid Content",
            LikeCount = 0
        };
        
        var post = await CreatePostAsync(newPost);

        var deletePostResponse = await TestClient.DeleteAsync($"api/Post/{post.PostId}");
        
        Assert.Equal(HttpStatusCode.Unauthorized, deletePostResponse.StatusCode);
    }

    [Fact]
    public async Task DeletePost_AuthorizedUser_DeletesPostFromDb()
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

        // Create post matching the created user
        var newPost = new PostCreateDto
        {
            UserId = userCreated.UserId,
            Content = "Valid Content",
            LikeCount = 0,
        };
        var createdPost = await CreatePostAsync(newPost);

        var loginRequest = new LoginRequestDto
        {
            Email = newUser.Email,
            Password = newUser.Password
        };
        var loginResponse = await TestClient.PostAsJsonAsync("api/User/login", loginRequest);
        var returnedUserResponse = await loginResponse.Content.ReadAsAsync<LoginResponseDto>();
        
        // Set jwt token for user
        TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", returnedUserResponse.Token);
        
        // Attempt deletion of newly created post
        var deleteResponse = await TestClient.DeleteAsync($"api/Post/{createdPost.PostId}");
        var responseMessage = await deleteResponse.Content.ReadAsStringAsync();
        
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        Assert.Equal("Post deleted successfully.", responseMessage);
    }
}