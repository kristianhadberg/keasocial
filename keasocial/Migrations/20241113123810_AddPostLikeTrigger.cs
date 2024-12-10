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
            -- Ensure the PostId exists before updating
            IF (SELECT COUNT(*) FROM Posts WHERE PostId = NEW.PostId) = 0 THEN
                SIGNAL SQLSTATE '45000' 
                SET MESSAGE_TEXT = 'No matching PostId found in Posts table';
            ELSE
                -- Increment the LikeCount for the corresponding Post
                UPDATE Posts
                SET LikeCount = LikeCount + 1
                WHERE PostId = NEW.PostId;
            END IF;
        END;
         ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS increment_like_count;");
        }
    }
}
