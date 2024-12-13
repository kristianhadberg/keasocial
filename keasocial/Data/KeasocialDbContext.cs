using keasocial.Models;
using Microsoft.EntityFrameworkCore;

namespace keasocial.Data
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
        /*public DbSet<PostLikeView> PostLikeViews { get; set; }*/

        public KeasocialDbContext(DbContextOptions<KeasocialDbContext> options)
            : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*modelBuilder.Entity<PostLikeView>()
                .ToView("PostLikesView")
                .HasNoKey();*/
        }
    }
}