using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class add_entity_topic_and_use_cup_as_join_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category_user_users_username",
                table: "category_user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_category_user",
                table: "category_user");

            migrationBuilder.DropIndex(
                name: "IX_category_user_username",
                table: "category_user");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "category_user",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "TagsCategoryKey",
                table: "category_user",
                type: "character varying(3)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TagsName",
                table: "category_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsersUsername",
                table: "category_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "topic_name",
                table: "category_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_category_user",
                table: "category_user",
                column: "id");

            migrationBuilder.CreateTable(
                name: "topics",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    category_key = table.Column<string>(type: "character varying(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topics", x => new { x.category_key, x.name });
                    table.ForeignKey(
                        name: "FK_topics_categories_category_key",
                        column: x => x.category_key,
                        principalTable: "categories",
                        principalColumn: "key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_category_user_category_key",
                table: "category_user",
                column: "category_key");

            migrationBuilder.CreateIndex(
                name: "IX_category_user_TagsCategoryKey_TagsName",
                table: "category_user",
                columns: new[] { "TagsCategoryKey", "TagsName" });

            migrationBuilder.CreateIndex(
                name: "IX_category_user_UsersUsername",
                table: "category_user",
                column: "UsersUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_category_user_topics_TagsCategoryKey_TagsName",
                table: "category_user",
                columns: new[] { "TagsCategoryKey", "TagsName" },
                principalTable: "topics",
                principalColumns: new[] { "category_key", "name" });

            migrationBuilder.AddForeignKey(
                name: "FK_category_user_users_UsersUsername",
                table: "category_user",
                column: "UsersUsername",
                principalTable: "users",
                principalColumn: "username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category_user_topics_TagsCategoryKey_TagsName",
                table: "category_user");

            migrationBuilder.DropForeignKey(
                name: "FK_category_user_users_UsersUsername",
                table: "category_user");

            migrationBuilder.DropTable(
                name: "topics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_category_user",
                table: "category_user");

            migrationBuilder.DropIndex(
                name: "IX_category_user_category_key",
                table: "category_user");

            migrationBuilder.DropIndex(
                name: "IX_category_user_TagsCategoryKey_TagsName",
                table: "category_user");

            migrationBuilder.DropIndex(
                name: "IX_category_user_UsersUsername",
                table: "category_user");

            migrationBuilder.DropColumn(
                name: "id",
                table: "category_user");

            migrationBuilder.DropColumn(
                name: "TagsCategoryKey",
                table: "category_user");

            migrationBuilder.DropColumn(
                name: "TagsName",
                table: "category_user");

            migrationBuilder.DropColumn(
                name: "UsersUsername",
                table: "category_user");

            migrationBuilder.DropColumn(
                name: "topic_name",
                table: "category_user");

            migrationBuilder.AddPrimaryKey(
                name: "PK_category_user",
                table: "category_user",
                columns: new[] { "category_key", "username" });

            migrationBuilder.CreateIndex(
                name: "IX_category_user_username",
                table: "category_user",
                column: "username");

            migrationBuilder.AddForeignKey(
                name: "FK_category_user_users_username",
                table: "category_user",
                column: "username",
                principalTable: "users",
                principalColumn: "username",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
