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
