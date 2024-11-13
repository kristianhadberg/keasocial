namespace keasocial.Dto;

public class PostCreateDto
{
    public int UserId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int LikeCount { get; set; }
    
}