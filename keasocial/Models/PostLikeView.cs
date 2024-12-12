using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace keasocial.Models;

public class PostLikeView
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
}