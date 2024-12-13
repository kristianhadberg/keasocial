namespace keasocial.Models;
public class User
{
    public string Uuid { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public ICollection<Post> Posts { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<CommentLike> CommentLikes { get; set; }
    public Calendar Calendar { get; set; }
}