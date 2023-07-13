using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class join_entity_user_idea_payload_interactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category_user_users_user_id",
                table: "category_user");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "category_user",
                newName: "username");

            migrationBuilder.RenameIndex(
                name: "IX_category_user_user_id",
                table: "category_user",
                newName: "IX_category_user_username");

            migrationBuilder.CreateTable(
                name: "user_idea",
                columns: table => new
                {
                    username = table.Column<string>(type: "text", nullable: false),
                    idea_id = table.Column<int>(type: "integer", nullable: false),
                    upvoted = table.Column<bool>(type: "boolean", nullable: false),
                    downvoted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_idea", x => new { x.username, x.idea_id });
                    table.ForeignKey(
                        name: "FK_user_idea_ideas_idea_id",
                        column: x => x.idea_id,
                        principalTable: "ideas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_idea_users_username",
                        column: x => x.username,
                        principalTable: "users",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_idea_idea_id",
                table: "user_idea",
                column: "idea_id");

            migrationBuilder.AddForeignKey(
                name: "FK_category_user_users_username",
                table: "category_user",
                column: "username",
                principalTable: "users",
                principalColumn: "username",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category_user_users_username",
                table: "category_user");

            migrationBuilder.DropTable(
                name: "user_idea");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "category_user",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_category_user_username",
                table: "category_user",
                newName: "IX_category_user_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_category_user_users_user_id",
                table: "category_user",
                column: "user_id",
                principalTable: "users",
                principalColumn: "username",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
