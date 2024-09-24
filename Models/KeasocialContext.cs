using Microsoft.EntityFrameworkCore;

namespace keasocial.Models
{
    public class SocialMediaContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=keasocial_db;Username=lasse;Password=1234");

        }
    }
}