using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace keasocial.Models;


public class Comment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int CommentId { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int LikeCount { get; set; }

    // Navigation properties
    public User User { get; set; }
    public Post Post { get; set; }
    public ICollection<CommentLike> CommentLikes { get; set; }
}