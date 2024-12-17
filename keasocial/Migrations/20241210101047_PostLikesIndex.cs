using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace keasocial.Migrations
{
    /// <inheritdoc />
    public partial class PostLikesIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Existing table and index creation...

            // Unique index on PostId and UserId in PostLikes
            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_User_Post",
                table: "PostLikes",
                columns: new[] { "PostId", "UserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the unique index
            migrationBuilder.DropIndex(
                name: "IX_PostLikes_User_Post",
                table: "PostLikes");
        }
    }
}
