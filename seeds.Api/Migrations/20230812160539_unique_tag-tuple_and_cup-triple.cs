using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class unique_tagtuple_and_cuptriple : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tags_category_key",
                table: "tags");

            migrationBuilder.DropIndex(
                name: "IX_category_user_category_key",
                table: "category_user");

            migrationBuilder.CreateIndex(
                name: "IX_tags_category_key_name",
                table: "tags",
                columns: new[] { "category_key", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_category_user_category_key_username_tag_name",
                table: "category_user",
                columns: new[] { "category_key", "username", "tag_name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tags_category_key_name",
                table: "tags");

            migrationBuilder.DropIndex(
                name: "IX_category_user_category_key_username_tag_name",
                table: "category_user");

            migrationBuilder.CreateIndex(
                name: "IX_tags_category_key",
                table: "tags",
                column: "category_key");

            migrationBuilder.CreateIndex(
                name: "IX_category_user_category_key",
                table: "category_user",
                column: "category_key");
        }
    }
}
