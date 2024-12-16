namespace keasocial.Dto;

public class CommentDto
{
    public string CommentId { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int LikeCount { get; set; }
    public List<CommentLikeDto> CommentLikes { get; set; } = new List<CommentLikeDto>();

}