using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace keasocial.Models;

public class Post
{
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int LikeCount {get; set; }

    // Navigation properties
    public User User { get; set; }
    
    public List<Comment> Comments { get; set; } = new List<Comment>();

    public ICollection<PostLike> PostLikes { get; set; }
}