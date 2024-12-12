using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace keasocial.Models;
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }


    // Navigation properties
    public ICollection<Post> Posts { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<CommentLike> CommentLikes { get; set; }
    public Calendar Calendar { get; set; }
}