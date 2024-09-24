namespace keasocial.Models;


public class Comment
{
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