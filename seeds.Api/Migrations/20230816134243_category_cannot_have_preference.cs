using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class category_cannot_have_preference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_preference_categories_CategoriesKey",
                table: "user_preference");

            migrationBuilder.DropIndex(
                name: "IX_user_preference_CategoriesKey",
                table: "user_preference");

            migrationBuilder.DropColumn(
                name: "CategoriesKey",
                table: "user_preference");

            migrationBuilder.CreateTable(
                name: "CategoryUser",
                columns: table => new
                {
                    CategoriesKey = table.Column<string>(type: "character varying(6)", nullable: false),
                    UsersUsername = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryUser", x => new { x.CategoriesKey, x.UsersUsername });
                    table.ForeignKey(
                        name: "FK_CategoryUser_categories_CategoriesKey",
                        column: x => x.CategoriesKey,
                        principalTable: "categories",
                        principalColumn: "key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryUser_users_UsersUsername",
                        column: x => x.UsersUsername,
                        principalTable: "users",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryUser_UsersUsername",
                table: "CategoryUser",
                column: "UsersUsername");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryUser");

            migrationBuilder.AddColumn<string>(
                name: "CategoriesKey",
                table: "user_preference",
                type: "character varying(6)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_user_preference_CategoriesKey",
                table: "user_preference",
                column: "CategoriesKey");

            migrationBuilder.AddForeignKey(
                name: "FK_user_preference_categories_CategoriesKey",
                table: "user_preference",
                column: "CategoriesKey",
                principalTable: "categories",
                principalColumn: "key",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
