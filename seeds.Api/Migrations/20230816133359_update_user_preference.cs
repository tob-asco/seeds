using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class update_user_preference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "category_user");

            migrationBuilder.CreateTable(
                name: "user_preference",
                columns: table => new
                {
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<int>(type: "integer", nullable: false),
                    CategoriesKey = table.Column<string>(type: "character varying(6)", nullable: false),
                    TagsId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_preference", x => new { x.item_id, x.username });
                    table.ForeignKey(
                        name: "FK_user_preference_categories_CategoriesKey",
                        column: x => x.CategoriesKey,
                        principalTable: "categories",
                        principalColumn: "key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_preference_topics_TagsId",
                        column: x => x.TagsId,
                        principalTable: "topics",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_user_preference_users_username",
                        column: x => x.username,
                        principalTable: "users",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_preference_CategoriesKey",
                table: "user_preference",
                column: "CategoriesKey");

            migrationBuilder.CreateIndex(
                name: "IX_user_preference_TagsId",
                table: "user_preference",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_user_preference_username",
                table: "user_preference",
                column: "username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_preference");

            migrationBuilder.CreateTable(
                name: "category_user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_key = table.Column<string>(type: "character varying(6)", nullable: false),
                    topic_name = table.Column<string>(type: "text", nullable: true),
                    TagsId = table.Column<Guid>(type: "uuid", nullable: true),
                    username = table.Column<string>(type: "text", nullable: false),
                    UsersUsername = table.Column<string>(type: "text", nullable: true),
                    value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_user", x => x.id);
                    table.ForeignKey(
                        name: "FK_category_user_categories_category_key",
                        column: x => x.category_key,
                        principalTable: "categories",
                        principalColumn: "key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_category_user_topics_TagsId",
                        column: x => x.TagsId,
                        principalTable: "topics",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_category_user_users_UsersUsername",
                        column: x => x.UsersUsername,
                        principalTable: "users",
                        principalColumn: "username");
                });

            migrationBuilder.CreateIndex(
                name: "IX_category_user_category_key_username_topic_name",
                table: "category_user",
                columns: new[] { "category_key", "username", "topic_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_category_user_TagsId",
                table: "category_user",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_category_user_UsersUsername",
                table: "category_user",
                column: "UsersUsername");
        }
    }
}
