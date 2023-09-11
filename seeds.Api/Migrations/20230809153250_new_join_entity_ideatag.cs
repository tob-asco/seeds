using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class new_join_entity_ideatopic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "idea_topic",
                columns: table => new
                {
                    idea_id = table.Column<int>(type: "integer", nullable: false),
                    category_key = table.Column<string>(type: "character varying(3)", nullable: false),
                    topic_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_idea_topic", x => new { x.idea_id, x.category_key, x.topic_name });
                    table.ForeignKey(
                        name: "FK_idea_topic_ideas_idea_id",
                        column: x => x.idea_id,
                        principalTable: "ideas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_idea_topic_topics_category_key_topic_name",
                        columns: x => new { x.category_key, x.topic_name },
                        principalTable: "topics",
                        principalColumns: new[] { "category_key", "name" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdeaTag",
                columns: table => new
                {
                    IdeasId = table.Column<int>(type: "integer", nullable: false),
                    TagsCategoryKey = table.Column<string>(type: "character varying(3)", nullable: false),
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
                name: "IX_IdeaTag_TagsCategoryKey_TagsName",
                table: "IdeaTag",
                columns: new[] { "TagsCategoryKey", "TagsName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "idea_topic");

            migrationBuilder.DropTable(
                name: "IdeaTag");
        }
    }
}
