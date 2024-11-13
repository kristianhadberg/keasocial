namespace keasocial.Models;

public class PostLike 
{
    public int PostLikeId { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }

    // Navigation properties
    public User? User { get; set; }
    public Post? Post { get; set; }
}
    

