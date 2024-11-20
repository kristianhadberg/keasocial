namespace keasocial.Dto;

public class PostDto
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int LikeCount {get; set; }
    public List<CommentDto> Comments { get; set; }

}