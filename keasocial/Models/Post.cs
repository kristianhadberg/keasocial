namespace keasocial.Models;

public class Post
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int LikeCount {get; set; }

    // Navigation properties
    public User User { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<PostLike> PostLikes { get; set; }
}