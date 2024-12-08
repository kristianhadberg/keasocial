using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace keasocial.Migrations
{
    /// <inheritdoc />
    public partial class AddPostLikeTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE TRIGGER increment_like_count
        AFTER INSERT ON PostLikes
        FOR EACH ROW
        BEGIN
            
            SELECT CASE 
               WHEN (SELECT COUNT(*) FROM Posts WHERE PostId = NEW.PostId) = 0 
               THEN RAISE(FAIL, 'No matching PostId found in Posts table') 
            END;
            
            UPDATE Posts
            SET LikeCount = LikeCount + 1
            WHERE PostId = NEW.PostId;
        END;
         ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS increment_like_count;");
        }
    }
}
