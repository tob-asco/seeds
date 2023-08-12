using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class category_key_up_to_6_chars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "category_key",
                table: "tags",
                type: "character varying(6)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)");

            migrationBuilder.AlterColumn<string>(
                name: "TagsCategoryKey",
                table: "IdeaTag",
                type: "character varying(6)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)");

            migrationBuilder.AlterColumn<string>(
                name: "category_key",
                table: "ideas",
                type: "character varying(6)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)");

            migrationBuilder.AlterColumn<string>(
                name: "category_key",
                table: "idea_tag",
                type: "character varying(6)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)");

            migrationBuilder.AlterColumn<string>(
                name: "category_key",
                table: "category_user",
                type: "character varying(6)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)");

            migrationBuilder.AlterColumn<string>(
                name: "TagsCategoryKey",
                table: "category_user",
                type: "character varying(6)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "key",
                table: "categories",
                type: "character varying(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "category_key",
                table: "tags",
                type: "character varying(3)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(6)");

            migrationBuilder.AlterColumn<string>(
                name: "TagsCategoryKey",
                table: "IdeaTag",
                type: "character varying(3)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(6)");

            migrationBuilder.AlterColumn<string>(
                name: "category_key",
                table: "ideas",
                type: "character varying(3)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(6)");

            migrationBuilder.AlterColumn<string>(
                name: "category_key",
                table: "idea_tag",
                type: "character varying(3)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(6)");

            migrationBuilder.AlterColumn<string>(
                name: "category_key",
                table: "category_user",
                type: "character varying(3)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(6)");

            migrationBuilder.AlterColumn<string>(
                name: "TagsCategoryKey",
                table: "category_user",
                type: "character varying(3)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "key",
                table: "categories",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(6)",
                oldMaxLength: 6);
        }
    }
}
