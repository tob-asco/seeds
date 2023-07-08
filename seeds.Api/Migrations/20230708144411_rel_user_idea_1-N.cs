using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class rel_user_idea_1N : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ideas_creator",
                table: "ideas",
                column: "creator");

            migrationBuilder.AddForeignKey(
                name: "FK_ideas_users_creator",
                table: "ideas",
                column: "creator",
                principalTable: "users",
                principalColumn: "username",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ideas_users_creator",
                table: "ideas");

            migrationBuilder.DropIndex(
                name: "IX_ideas_creator",
                table: "ideas");
        }
    }
}
