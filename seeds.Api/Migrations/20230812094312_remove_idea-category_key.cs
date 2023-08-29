using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class remove_ideacategory_key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "category_key",
                table: "ideas",
                type: "character varying(6)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ideas_category_key",
                table: "ideas",
                column: "category_key");

            migrationBuilder.AddForeignKey(
                name: "FK_ideas_categories_category_key",
                table: "ideas",
                column: "category_key",
                principalTable: "categories",
                principalColumn: "key");
        }
    }
}
