using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace keasocial.Models;

public class CommentLike
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int CommentLikeId { get; set; }
    public int UserId { get; set; }
    public int CommentId { get; set; }

    // Navigation properties
    public User User { get; set; }
    public Comment Comment { get; set; }
}
