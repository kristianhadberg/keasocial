namespace keasocial.Dto;

public class CommentDto
{
    public string Uuid { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int LikeCount { get; set; }
}