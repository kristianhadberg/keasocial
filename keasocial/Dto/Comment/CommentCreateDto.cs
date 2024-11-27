namespace keasocial.Dto;

public class CommentCreateDto
{
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string Content { get; set; }
    public int LikeCount { get; set; }
}
