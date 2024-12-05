using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace keasocial.Migrations
{
    /// <inheritdoc />
    public partial class AddPostLikesView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE VIEW PostLikesView AS
        SELECT 
            P.PostId,
            U.UserId,
            U.Name
        FROM 
            Posts P
        JOIN 
            PostLikes PL ON P.PostId = PL.PostId
        JOIN 
            Users U ON PL.UserId = U.UserId;
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS PostLikesView;");
        }
    }
}
