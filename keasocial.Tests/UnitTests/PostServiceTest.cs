using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
using keasocial.Services;
using Moq;

namespace keasocial.Tests.UnitTests;

public class PostServiceTest
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly PostService _postService;

    public PostServiceTest()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _postService = new PostService(_postRepositoryMock.Object);
    }

    [Theory]
    [InlineData("hello")] // 5 chars
    [InlineData("helloo")] // 6 chars
    [InlineData("I like this post very much :)")] 
    [InlineData("tenletterstenletterstenletterstenletterstenletters" +
                "tenletterstenletterstenletterstenletterstenletter")] // 99 chars
    [InlineData("tenletterstenletterstenletterstenletterstenletters" +
                "tenletterstenletterstenletterstenletterstenletters")] // 100 chars 
    public void ValidatePostCreateDto_ValidContentLength_ThrowsNoException(string content)
    {
        var postCreateDto = new PostCreateDto
        {
            UserId = 1,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            LikeCount = 0
        };

        var exception = Record.Exception(() => _postService.ValidatePostContent(postCreateDto.Content));
        
        Assert.Null(exception);
    }
    
    [Theory]
    [InlineData("")] // 0 chars
    [InlineData("hell")] // 4 chars
    [InlineData("tenletterstenletterstenletterstenletterstenletters" +
                "tenletterstenletterstenletterstenletterstenletters1")] // 101 chars 
    public void ValidatePostCreateDto_TooShortOrLongContent_ThrowsException(string content)
    {
        var postCreateDto = new PostCreateDto
        {
            UserId = 1,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            LikeCount = 0
        };

        var exception = Record.Exception(() => _postService.ValidatePostContent(postCreateDto.Content));
        
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public async void GetAsync_ValidId_ReturnsPost()
    {
        var expectedPost = new Post { PostId = 1, Content = "Valid Content" };
        _postRepositoryMock.Setup(repo => repo.GetAsync(1))
            .ReturnsAsync(expectedPost);

        var result = await  _postService.GetAsync(1);

        Assert.NotNull(result);
        Assert.Equal(expectedPost.Content, result.Content);
    }

    [Fact]
    public async void GetAsync_NonExistingId_ReturnsNull()
    {
        _postRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
            .ReturnsAsync((Post)null);

        var result = await _postService.GetAsync(99);
        
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_ReturnsListOfPosts()
    {
        var expectedPosts = new List<PostDto>
        {
            new PostDto { PostId = 1, Content = "First Post" },
            new PostDto { PostId = 2, Content = "Second Post" }
        };

        _postRepositoryMock.Setup(repo => repo.GetAsync()).ReturnsAsync(expectedPosts);

        var result = await _postService.GetAsync();
        
        Assert.Equal(2, result.Count);
        Assert.IsType<PostDto>(result.First());
    }
    
    [Fact]
    public async Task GetAsync_EmptyList_ReturnsEmptyList()
    {
        _postRepositoryMock.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<PostDto>());

        var result = await _postService.GetAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task CreateAsync_ValidPost_ReturnsPost()
    {
        var postCreateDto = new PostCreateDto
        {
            UserId = 1,
            Content = "Valid Content",
            CreatedAt = DateTime.UtcNow,
            LikeCount = 0
        };

        var postExpected = new Post
        {
            UserId = postCreateDto.UserId,
            Content = postCreateDto.Content,
            CreatedAt = postCreateDto.CreatedAt,
            LikeCount = postCreateDto.LikeCount
        };

        _postRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Post>())).ReturnsAsync(postExpected);

        var result = await _postService.CreateAsync(postCreateDto);
        
        Assert.Equal(postCreateDto.UserId, result.UserId);
        Assert.Equal(postCreateDto.Content, result.Content);
    }
    
    [Fact]
    public async Task UpdateAsync_PostDoesNotExist_ThrowsException()
    {
        // Arrange
        int postId = 1;
        int userId = 1;
        var postUpdateDto = new PostUpdateDto { Content = "Updated content", LikeCount = 0 };
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _postService.UpdateAsync(postId, postUpdateDto, userId));
        Assert.Equal($"Post with ID {postId} not found.", exception.Message);
    }
    
    [Fact]
    public async Task UpdateAsync_UnauthorizedUser_ThrowsUnauthorizedException()
    {
        var postId = 1;
        var post = new Post { PostId = postId, UserId = 100, Content = "Valid Content" };
        var postUpdateDto = new PostUpdateDto { Content = "Updated content", LikeCount = 0 };
        _postRepositoryMock.Setup(repo => repo.GetAsync(postId)).ReturnsAsync(post);

        var exception = await Record.ExceptionAsync(() => _postService.UpdateAsync(post.PostId, postUpdateDto, postId));
        Assert.IsType<UnauthorizedAccessException>(exception);
    }
    
    [Fact]
    public async Task UpdateAsync_Valid_ReturnsPost()
    {
        int postId = 1;
        int userId = 1;
        var postUpdateDto = new PostUpdateDto { Content = "Updated content", LikeCount = 10 };
        var existingPost = new Post { PostId = postId, UserId = userId, Content = "Original content" };
        var updatedPost = new Post { PostId = postId, UserId = userId, Content = postUpdateDto.Content, LikeCount = postUpdateDto.LikeCount };

        _postRepositoryMock.Setup(r => r.GetAsync(postId)).ReturnsAsync(existingPost);
        _postRepositoryMock.Setup(r => r.UpdateAsync(postId, It.IsAny<Post>())).ReturnsAsync(updatedPost);

        var result = await _postService.UpdateAsync(postId, postUpdateDto, userId);

        Assert.Equal(postUpdateDto.Content, result.Content);
        Assert.Equal(postUpdateDto.LikeCount, result.LikeCount);
        _postRepositoryMock.Verify(r => r.UpdateAsync(postId, It.IsAny<Post>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ValidUserIdAndPostId_DeletesPost()
    {
        var postId = 1;
        var userId = 1;

        var post = new Post { PostId = postId, UserId = userId, Content = "Valid Content" };

        _postRepositoryMock.Setup(repo => repo.GetAsync(postId)).ReturnsAsync(post);
        _postRepositoryMock.Setup(repo => repo.DeleteAsync(postId)).ReturnsAsync(post);

        var result =  _postService.DeleteAsync(userId, postId); // this is missing an await

        Assert.NotNull(result);
        Assert.Equal(postId, result.Id); // Should be result.PostId
    }
    
    [Fact]
    public async Task DeleteAsync_PostNotFound_ThrowsException()
    {
        var postId = 1;
        _postRepositoryMock.Setup(repo => repo.GetAsync(postId)).ReturnsAsync((Post) null);
        
        var exception = await Record.ExceptionAsync(() => _postService.DeleteAsync(123, postId));

        Assert.IsType<KeyNotFoundException>(exception);
    }

    [Fact]
    public async Task DeleteAsync_UnauthorizedUser_ThrowsUnauthorizedException()
    {
        var postId = 1;
        var post = new Post { PostId = postId, UserId = 100, Content = "Valid Content" };
        _postRepositoryMock.Setup(repo => repo.GetAsync(postId)).ReturnsAsync(post);

        var exception = await Record.ExceptionAsync(() => _postService.DeleteAsync(200, postId));
        Assert.IsType<UnauthorizedAccessException>(exception);
    }
    
}