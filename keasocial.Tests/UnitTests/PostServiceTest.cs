using keasocial.Dto;
using keasocial.Repositories.Interfaces;
using keasocial.Services;
using Moq;

namespace keasocial.Tests;

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

        var exception = Record.Exception(() => _postService.ValidatePostCreateDto(postCreateDto));
        
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

        var exception = Record.Exception(() => _postService.ValidatePostCreateDto(postCreateDto));
        
        Assert.IsType<ArgumentException>(exception);
    }
}