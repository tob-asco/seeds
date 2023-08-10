using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class category_key_nullable_in_idea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ideas_categories_category_key",
                table: "ideas");

            migrationBuilder.AlterColumn<string>(
                name: "category_key",
                table: "ideas",
                type: "character varying(6)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(6)");

            migrationBuilder.AddForeignKey(
                name: "FK_ideas_categories_category_key",
                table: "ideas",
                column: "category_key",
                principalTable: "categories",
                principalColumn: "key");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ideas_categories_category_key",
                table: "ideas");

            migrationBuilder.AlterColumn<string>(
                name: "category_key",
                table: "ideas",
                type: "character varying(6)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(6)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ideas_categories_category_key",
                table: "ideas",
                column: "category_key",
                principalTable: "categories",
                principalColumn: "key",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
