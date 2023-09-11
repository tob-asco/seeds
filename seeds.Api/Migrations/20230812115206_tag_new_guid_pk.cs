using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class topic_new_guid_pk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category_user_topics_TagsCategoryKey_TagsName",
                table: "category_user");

            migrationBuilder.DropForeignKey(
                name: "FK_idea_topic_topics_category_key_topic_name",
                table: "idea_topic");

            migrationBuilder.DropTable(
                name: "IdeaTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_topics",
                table: "topics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_idea_topic",
                table: "idea_topic");

            migrationBuilder.DropIndex(
                name: "IX_idea_topic_category_key_topic_name",
                table: "idea_topic");

            migrationBuilder.DropIndex(
                name: "IX_category_user_TagsCategoryKey_TagsName",
                table: "category_user");

            migrationBuilder.DropColumn(
                name: "category_key",
                table: "idea_topic");

            migrationBuilder.DropColumn(
                name: "topic_name",
                table: "idea_topic");

            migrationBuilder.DropColumn(
                name: "TagsCategoryKey",
                table: "category_user");

            migrationBuilder.DropColumn(
                name: "TagsName",
                table: "category_user");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "topics",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "topic_id",
                table: "idea_topic",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TagsId",
                table: "category_user",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_topics",
                table: "topics",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_idea_topic",
                table: "idea_topic",
                columns: new[] { "idea_id", "topic_id" });

            migrationBuilder.CreateIndex(
                name: "IX_topics_category_key",
                table: "topics",
                column: "category_key");

            migrationBuilder.CreateIndex(
                name: "IX_idea_topic_topic_id",
                table: "idea_topic",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "IX_category_user_TagsId",
                table: "category_user",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_category_user_topics_TagsId",
                table: "category_user",
                column: "TagsId",
                principalTable: "topics",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_idea_topic_topics_topic_id",
                table: "idea_topic",
                column: "topic_id",
                principalTable: "topics",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category_user_topics_TagsId",
                table: "category_user");

            migrationBuilder.DropForeignKey(
                name: "FK_idea_topic_topics_topic_id",
                table: "idea_topic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_topics",
                table: "topics");

            migrationBuilder.DropIndex(
                name: "IX_topics_category_key",
                table: "topics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_idea_topic",
                table: "idea_topic");

            migrationBuilder.DropIndex(
                name: "IX_idea_topic_topic_id",
                table: "idea_topic");

            migrationBuilder.DropIndex(
                name: "IX_category_user_TagsId",
                table: "category_user");

            migrationBuilder.DropColumn(
                name: "id",
                table: "topics");

            migrationBuilder.DropColumn(
                name: "topic_id",
                table: "idea_topic");

            migrationBuilder.DropColumn(
                name: "TagsId",
                table: "category_user");

            migrationBuilder.AddColumn<string>(
                name: "category_key",
                table: "idea_topic",
                type: "character varying(6)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "topic_name",
                table: "idea_topic",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TagsCategoryKey",
                table: "category_user",
                type: "character varying(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TagsName",
                table: "category_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_topics",
                table: "topics",
                columns: new[] { "category_key", "name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_idea_topic",
                table: "idea_topic",
                columns: new[] { "idea_id", "category_key", "topic_name" });

            migrationBuilder.CreateTable(
                name: "IdeaTag",
                columns: table => new
                {
                    IdeasId = table.Column<int>(type: "integer", nullable: false),
                    TagsCategoryKey = table.Column<string>(type: "character varying(6)", nullable: false),
                    TagsName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaTag", x => new { x.IdeasId, x.TagsCategoryKey, x.TagsName });
                    table.ForeignKey(
                        name: "FK_IdeaTag_ideas_IdeasId",
                        column: x => x.IdeasId,
                        principalTable: "ideas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdeaTag_topics_TagsCategoryKey_TagsName",
                        columns: x => new { x.TagsCategoryKey, x.TagsName },
                        principalTable: "topics",
                        principalColumns: new[] { "category_key", "name" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_idea_topic_category_key_topic_name",
                table: "idea_topic",
                columns: new[] { "category_key", "topic_name" });

            migrationBuilder.CreateIndex(
                name: "IX_category_user_TagsCategoryKey_TagsName",
                table: "category_user",
                columns: new[] { "TagsCategoryKey", "TagsName" });

            migrationBuilder.CreateIndex(
                name: "IX_IdeaTag_TagsCategoryKey_TagsName",
                table: "IdeaTag",
                columns: new[] { "TagsCategoryKey", "TagsName" });

            migrationBuilder.AddForeignKey(
                name: "FK_category_user_topics_TagsCategoryKey_TagsName",
                table: "category_user",
                columns: new[] { "TagsCategoryKey", "TagsName" },
                principalTable: "topics",
                principalColumns: new[] { "category_key", "name" });

            migrationBuilder.AddForeignKey(
                name: "FK_idea_topic_topics_category_key_topic_name",
                table: "idea_topic",
                columns: new[] { "category_key", "topic_name" },
                principalTable: "topics",
                principalColumns: new[] { "category_key", "name" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
