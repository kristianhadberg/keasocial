using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace keasocial.Migrations
{
    /// <inheritdoc />
    public partial class AddMostLikedCommentForUserFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS GetMostLikedCommentForUser;

                CREATE FUNCTION GetMostLikedCommentForUser(UserId INT)
                RETURNS INT
                DETERMINISTIC
                READS SQL DATA
                BEGIN
                    DECLARE MostLikedCommentId INT;
                    
	                SELECT C.CommentId INTO MostLikedCommentId
                    FROM Comments C
                    ORDER BY C.LikeCount DESC
                    LIMIT 1;

                    RETURN MostLikedCommentId;
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS GetMostLikedCommentForUser;
            ");
        }
    }
}
