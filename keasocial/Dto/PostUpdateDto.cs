namespace keasocial.Dto;

public class PostUpdateDto
{
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int LikeCount { get; set; }
}