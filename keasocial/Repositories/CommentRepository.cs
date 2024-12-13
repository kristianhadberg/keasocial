using keasocial.Data;
using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
using Neo4jClient;

namespace keasocial.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly IGraphClient _graphClient;

    public CommentRepository(IGraphClient graphClient)
    {
        _graphClient = graphClient;
    }
    
    public async Task<Comment> GetAsync(string commentUuid, string postUuid)
    {
        var comment = await _graphClient.Cypher
            .Match("(p:Post {Uuid: $postUuid})-[:HAS_COMMENT]->(c:Comment {Uuid: $commentUuid})")
            .WithParams(new { commentUuid, postUuid})
            .Return<Comment>("c")
            .ResultsAsync;

        return comment.FirstOrDefault();
    }
    
    public async Task<List<CommentDto>> GetAsync()
    {
        var result = await _graphClient.Cypher
            .Match("(c:Comment)")
            .Return<CommentDto>("c")
            .ResultsAsync;
        return result.ToList();
    }
    
    public async Task<IEnumerable<CommentDto>> GetByPostIdAsync(string postUuid)
    {
        var comments = await _graphClient.Cypher
            .Match("(p:Post {Uuid: $postUuid})-[:HAS_COMMENT]->(c:Comment)")
            .WithParams(new { postUuid })
            .Return<CommentDto>("c")
            .ResultsAsync;

        return comments.ToList();
    }
    
    public async Task<CommentDto> CreateAsync(Comment comment, string postUuid, string userUuid)
    {
        var newComment = await _graphClient.Cypher
            .Create("(c:Comment {Uuid: randomUUID(), Content: $content, CreatedAt: $createdAt, LikeCount: $likeCount})")
            .WithParams(new
            {
                content = comment.Content,
                createdAt = comment.CreatedAt,
                likeCount = comment.LikeCount
            })
            .Return<CommentDto>("c")
            .ResultsAsync;
            
        var createdComment = newComment.FirstOrDefault();
        if (createdComment != null)
        {
            // Create relationships: User -> Comment & Post -> Comment
            await _graphClient.Cypher
                .Match("(u:User {Uuid: $userUuid})", "(p:Post {Uuid: $postUuid})", "(c:Comment {Uuid: $commentUuid})")
                .Where((Comment c) => c.Uuid == createdComment.Uuid)
                .Create("(u)-[:COMMENTED]->(c)")
                .Create("(p)-[:HAS_COMMENT]->(c)")
                .WithParams(new
                {
                    commentUuid = createdComment.Uuid,
                    userUuid = userUuid,
                    postUuid = postUuid
                })
                .ExecuteWithoutResultsAsync();
        }

        return createdComment; 
    }

    public async Task<Comment> UpdateAsync(string uuid, Comment comment)
    {
        var updatedComment = await _graphClient.Cypher
            .Match("(c:Comment {Uuid: $uuid})")
            .Set("c.Content = $content, c.CreatedAt = $createdAt, c.LikeCount = $likeCount")
            .WithParams(new
            {
                uuid = uuid,
                content = comment.Content,
                createdAt = comment.CreatedAt,
                likeCount = comment.LikeCount
            })
            .Return<Comment>("c")
            .ResultsAsync;

        return updatedComment.FirstOrDefault();
    }

    public async Task<Comment> DeleteAsync(string uuid)
    {
        var deletedComment = await _graphClient.Cypher
            .Match("(c:Comment {Uuid: $uuid})")
            .WithParams(new { uuid })
            .DetachDelete("c")
            .Return<Comment>("c") 
            .ResultsAsync;

        return deletedComment.FirstOrDefault();
    }

    public async Task<bool> AddCommentLikeAsync(string commentUuid, string userUuid)
    {
        var existingLike = await _graphClient.Cypher
            .Match("(u:User {Uuid: $userUuid})-[r:LIKED]->(c:Comment {Uuid: $commentUuid})")
            .WithParams(new { userUuid, commentUuid })
            .Return<int>("count(r)")
            .ResultsAsync;

        if (existingLike.FirstOrDefault() > 0)
        {
            return false;
        }
        
        await _graphClient.Cypher
            .Match("(u:User {Uuid: $userUuid}), (c:Comment {Uuid: $commentUuid})")
            .WithParams(new { userUuid, commentUuid })
            .Create("(u)-[:LIKED]->(c)")
            .ExecuteWithoutResultsAsync();

        return true;
    }
    
    public async Task<bool> IsUserAuthorizedToChangeComment(string userUuid, string commentUuid)
    {
        var userPostCheck = await _graphClient.Cypher
            .Match("(u:User {Uuid: $userUuid})-[:COMMENTED]->(c:Comment {Uuid: $commentUuid})")
            .WithParams(new { userUuid, commentUuid })
            .Return<int>("count(c)")
            .ResultsAsync;

        return userPostCheck.FirstOrDefault() > 0;
    }
    
}
