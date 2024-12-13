using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
using Neo4jClient;

namespace keasocial.Repositories;

public class PostRepository : IPostRepository
{
    private readonly IGraphClient _graphClient;

    public PostRepository(IGraphClient graphClient)
    {
        _graphClient = graphClient;
    }

    public async Task<Post> GetAsync(string uuid)
    {
        var result = await _graphClient.Cypher
            .Match("(p:Post)")
            .Where((Post p) => p.Uuid == uuid)
            .OptionalMatch("(p)-[:HAS_COMMENT]->(c:Comment)")
            .Return((p, c) => new
            {
                Post = p.As<Post>(),
                Comments = c.CollectAs<Comment>(),
            })
            .ResultsAsync;

        var data = result.FirstOrDefault();

        if (data == null) return null;


        var post = data.Post;
        post.Comments = data.Comments.ToList();
        return post;
    }

    public async Task<List<PostDto>> GetAsync()
    {
        var posts = await _graphClient.Cypher
            .Match("(p:Post)")
            .Return<PostDto>("p")
            .ResultsAsync;

        return posts.ToList();
    }
    
    public async Task<Post> CreateAsync(Post post, string userUuid)
    {
        var newPost = await _graphClient.Cypher
            .Create("(p: Post {Uuid: randomUUID(), Content: $content, CreatedAt: $createdAt, LikeCount: $likeCount})")
            .WithParams(new
            {
                content = post.Content,
                createdAt = post.CreatedAt,
                likeCount = post.LikeCount
            })
            .Return<Post>("p")
            .ResultsAsync;

        var createdPost = newPost.FirstOrDefault();

        if (createdPost != null)
        {
            await _graphClient.Cypher
                .Match("(u:User {Uuid: $userUuid})", "(p:Post {Uuid: $postUuid})")
                .Where((Post p) => p.Uuid == createdPost.Uuid)
                .Create("(u)-[:POSTED]->(p)")
                .WithParams(new
                {
                    userUuid = userUuid,
                    postUuid = createdPost.Uuid
                })
                .ExecuteWithoutResultsAsync();
        }

        return createdPost;
    }
    
    public async Task<Post> UpdateAsync(string uuid, Post post)
    {
        var updatedPost = await _graphClient.Cypher
            .Match("(p:Post {Uuid: $postUuid})")
            .Set("p.Content = $content, p.CreatedAt = $createdAt, p.LikeCount = $likeCount")
            .WithParams(new
            {
                postUuid = uuid,
                content = post.Content,
                createdAt = post.CreatedAt,
                likeCount = post.LikeCount
            })
            .Return<Post>("p")
            .ResultsAsync;

        return updatedPost.FirstOrDefault();
    }
    
    public async Task<Post> DeleteAsync(string uuid)
    {
        var deletedPost = await _graphClient.Cypher
            .Match("(p:Post {Uuid: $uuid})")
            .WithParams(new { uuid })
            .DetachDelete("p")
            .Return<Post>("p") 
            .ResultsAsync;

        return deletedPost.FirstOrDefault();
    }

    public async Task<bool> AddPostLikeAsync(string postUuid, string userUuid)
    {
        var existingLike = await _graphClient.Cypher
            .Match("(u:User {Uuid: $userUuid})-[r:LIKED]->(p:Post {Uuid: $postUuid})")
            .WithParams(new { userUuid, postUuid })
            .Return<int>("count(r)") // Check if the relationship exists
            .ResultsAsync;

        if (existingLike.FirstOrDefault() > 0)
        {
            return false;
        }
        
        await _graphClient.Cypher
            .Match("(u:User {Uuid: $userUuid}), (p:Post {Uuid: $postUuid})")
            .WithParams(new { userUuid, postUuid })
            .Create("(u)-[:LIKED]->(p)")
            .ExecuteWithoutResultsAsync();

        return true;
    }

    public async Task<bool> IsUserAuthorizedToChangePost(string userUuid, string postUuid)
    {
        var userPostCheck = await _graphClient.Cypher
            .Match("(u:User {Uuid: $userUuid})-[:POSTED]->(p:Post {Uuid: $postUuid})")
            .WithParams(new { userUuid, postUuid })
            .Return<int>("count(p)")
            .ResultsAsync;

        return userPostCheck.FirstOrDefault() > 0;
    }

    private CommentDto CommentToDto(Comment comment)
    {
        return new CommentDto
        {
            CommentId = comment.CommentId,
            UserId = comment.UserId,
            PostId = comment.PostId,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            LikeCount = comment.LikeCount,
            CommentLikes = comment.CommentLikes.Select(CommentLikeToDto).ToList()
        };
    }
    
    private CommentLikeDto CommentLikeToDto(CommentLike commentLike)
    {
        return new CommentLikeDto
        {
            CommentLikeId = commentLike.CommentLikeId,
            UserId = commentLike.UserId,
            CommentId = commentLike.CommentId
        };
    }
}