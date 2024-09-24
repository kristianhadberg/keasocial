namespace keasocial.Models;

public class CommentLike
{
    public int CommentLikeId { get; set; }
    public int UserId { get; set; }
    public int CommentId { get; set; }

    // Navigation properties
    public User User { get; set; }
    public Comment Comment { get; set; }
}
