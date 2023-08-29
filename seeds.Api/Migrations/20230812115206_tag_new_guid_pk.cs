using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class tag_new_guid_pk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category_user_tags_TagsCategoryKey_TagsName",
                table: "category_user");

            migrationBuilder.DropForeignKey(
                name: "FK_idea_tag_tags_category_key_tag_name",
                table: "idea_tag");

            migrationBuilder.DropTable(
                name: "IdeaTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tags",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_idea_tag",
                table: "idea_tag");

            migrationBuilder.DropIndex(
                name: "IX_idea_tag_category_key_tag_name",
                table: "idea_tag");

            migrationBuilder.DropIndex(
                name: "IX_category_user_TagsCategoryKey_TagsName",
                table: "category_user");

            migrationBuilder.DropColumn(
                name: "category_key",
                table: "idea_tag");

            migrationBuilder.DropColumn(
                name: "tag_name",
                table: "idea_tag");

            migrationBuilder.DropColumn(
                name: "TagsCategoryKey",
                table: "category_user");

            migrationBuilder.DropColumn(
                name: "TagsName",
                table: "category_user");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "tags",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "tag_id",
                table: "idea_tag",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TagsId",
                table: "category_user",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tags",
                table: "tags",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_idea_tag",
                table: "idea_tag",
                columns: new[] { "idea_id", "tag_id" });

            migrationBuilder.CreateIndex(
                name: "IX_tags_category_key",
                table: "tags",
                column: "category_key");

            migrationBuilder.CreateIndex(
                name: "IX_idea_tag_tag_id",
                table: "idea_tag",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_category_user_TagsId",
                table: "category_user",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_category_user_tags_TagsId",
                table: "category_user",
                column: "TagsId",
                principalTable: "tags",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_idea_tag_tags_tag_id",
                table: "idea_tag",
                column: "tag_id",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category_user_tags_TagsId",
                table: "category_user");

            migrationBuilder.DropForeignKey(
                name: "FK_idea_tag_tags_tag_id",
                table: "idea_tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tags",
                table: "tags");

            migrationBuilder.DropIndex(
                name: "IX_tags_category_key",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_idea_tag",
                table: "idea_tag");

            migrationBuilder.DropIndex(
                name: "IX_idea_tag_tag_id",
                table: "idea_tag");

            migrationBuilder.DropIndex(
                name: "IX_category_user_TagsId",
                table: "category_user");

            migrationBuilder.DropColumn(
                name: "id",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "tag_id",
                table: "idea_tag");

            migrationBuilder.DropColumn(
                name: "TagsId",
                table: "category_user");

            migrationBuilder.AddColumn<string>(
                name: "category_key",
                table: "idea_tag",
                type: "character varying(6)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "tag_name",
                table: "idea_tag",
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
                name: "PK_tags",
                table: "tags",
                columns: new[] { "category_key", "name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_idea_tag",
                table: "idea_tag",
                columns: new[] { "idea_id", "category_key", "tag_name" });

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
                        name: "FK_IdeaTag_tags_TagsCategoryKey_TagsName",
                        columns: x => new { x.TagsCategoryKey, x.TagsName },
                        principalTable: "tags",
                        principalColumns: new[] { "category_key", "name" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_idea_tag_category_key_tag_name",
                table: "idea_tag",
                columns: new[] { "category_key", "tag_name" });

            migrationBuilder.CreateIndex(
                name: "IX_category_user_TagsCategoryKey_TagsName",
                table: "category_user",
                columns: new[] { "TagsCategoryKey", "TagsName" });

            migrationBuilder.CreateIndex(
                name: "IX_IdeaTag_TagsCategoryKey_TagsName",
                table: "IdeaTag",
                columns: new[] { "TagsCategoryKey", "TagsName" });

            migrationBuilder.AddForeignKey(
                name: "FK_category_user_tags_TagsCategoryKey_TagsName",
                table: "category_user",
                columns: new[] { "TagsCategoryKey", "TagsName" },
                principalTable: "tags",
                principalColumns: new[] { "category_key", "name" });

            migrationBuilder.AddForeignKey(
                name: "FK_idea_tag_tags_category_key_tag_name",
                table: "idea_tag",
                columns: new[] { "category_key", "tag_name" },
                principalTable: "tags",
                principalColumns: new[] { "category_key", "name" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
