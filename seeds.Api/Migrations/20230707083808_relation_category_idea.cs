using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class relation_category_idea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tags",
                table: "categories");

            migrationBuilder.AddColumn<string>(
                name: "category_key",
                table: "ideas",
                type: "character varying(3)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ideas_category_key",
                table: "ideas",
                column: "category_key");

            migrationBuilder.AddForeignKey(
                name: "FK_ideas_categories_category_key",
                table: "ideas",
                column: "category_key",
                principalTable: "categories",
                principalColumn: "key",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ideas_categories_category_key",
                table: "ideas");

            migrationBuilder.DropIndex(
                name: "IX_ideas_category_key",
                table: "ideas");

            migrationBuilder.DropColumn(
                name: "category_key",
                table: "ideas");

            migrationBuilder.AddColumn<List<string>>(
                name: "tags",
                table: "categories",
                type: "text[]",
                nullable: false);
        }
    }
}
