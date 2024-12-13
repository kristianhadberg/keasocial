namespace keasocial.Models;

public class Post
{
    public string Uuid { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int LikeCount {get; set; }

    // Navigation properties
    public ICollection<Comment> Comments { get; set; }
}