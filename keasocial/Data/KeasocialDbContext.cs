using Microsoft.EntityFrameworkCore;

namespace keasocial.Models
{
    public class KeasocialDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }

        public KeasocialDbContext(DbContextOptions<KeasocialDbContext> options)
            : base(options) {}
    }
}