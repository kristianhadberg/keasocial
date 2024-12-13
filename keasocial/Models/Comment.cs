namespace keasocial.Models;


public class Comment
{
    public string Uuid { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int LikeCount { get; set; }
}