using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace keasocial.Models;

public class PostLike 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int PostLikeId { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }

    // Navigation properties
    public User? User { get; set; }
    public Post? Post { get; set; }
}
    

