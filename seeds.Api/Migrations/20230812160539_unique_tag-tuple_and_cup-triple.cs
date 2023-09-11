using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class unique_topictuple_and_cuptriple : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_topics_category_key",
                table: "topics");

            migrationBuilder.DropIndex(
                name: "IX_category_user_category_key",
                table: "category_user");

            migrationBuilder.CreateIndex(
                name: "IX_topics_category_key_name",
                table: "topics",
                columns: new[] { "category_key", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_category_user_category_key_username_topic_name",
                table: "category_user",
                columns: new[] { "category_key", "username", "topic_name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_topics_category_key_name",
                table: "topics");

            migrationBuilder.DropIndex(
                name: "IX_category_user_category_key_username_topic_name",
                table: "category_user");

            migrationBuilder.CreateIndex(
                name: "IX_topics_category_key",
                table: "topics",
                column: "category_key");

            migrationBuilder.CreateIndex(
                name: "IX_category_user_category_key",
                table: "category_user",
                column: "category_key");
        }
    }
}
