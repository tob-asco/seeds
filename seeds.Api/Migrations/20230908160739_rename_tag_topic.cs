using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class rename_tag_topic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_preference_topics_TagsId",
                table: "user_preference");

            migrationBuilder.RenameColumn(
                name: "TagsId",
                table: "user_preference",
                newName: "TopicsId");

            migrationBuilder.RenameIndex(
                name: "IX_user_preference_TagsId",
                table: "user_preference",
                newName: "IX_user_preference_TopicsId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_preference_topics_TopicsId",
                table: "user_preference",
                column: "TopicsId",
                principalTable: "topics",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_preference_topics_TopicsId",
                table: "user_preference");

            migrationBuilder.RenameColumn(
                name: "TopicsId",
                table: "user_preference",
                newName: "TagsId");

            migrationBuilder.RenameIndex(
                name: "IX_user_preference_TopicsId",
                table: "user_preference",
                newName: "IX_user_preference_TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_preference_topics_TagsId",
                table: "user_preference",
                column: "TagsId",
                principalTable: "topics",
                principalColumn: "id");
        }
    }
}
