using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace keasocial.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGetMostLikedPostStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS GetMostLikedPosts;
                CREATE PROCEDURE GetMostLikedPosts()
                BEGIN
                    SELECT *
                    FROM Posts
                    WHERE CreatedAt >= NOW() - INTERVAL 7 DAY
                    ORDER BY LikeCount DESC
                    LIMIT 10;
                END;           
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS GetMostLikedPosts;
            ");
        }
    }
}
