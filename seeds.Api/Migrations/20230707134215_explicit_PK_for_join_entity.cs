using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class explicit_PK_for_join_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category_user_users_UsersUsername",
                table: "category_user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_category_user",
                table: "category_user");

            migrationBuilder.DropIndex(
                name: "IX_category_user_UsersUsername",
                table: "category_user");

            migrationBuilder.DropColumn(
                name: "UsersUsername",
                table: "category_user");

            migrationBuilder.AddPrimaryKey(
                name: "PK_category_user",
                table: "category_user",
                columns: new[] { "category_key", "user_id" });

            migrationBuilder.CreateIndex(
                name: "IX_category_user_user_id",
                table: "category_user",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_category_user_users_user_id",
                table: "category_user",
                column: "user_id",
                principalTable: "users",
                principalColumn: "username",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category_user_users_user_id",
                table: "category_user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_category_user",
                table: "category_user");

            migrationBuilder.DropIndex(
                name: "IX_category_user_user_id",
                table: "category_user");

            migrationBuilder.AddColumn<string>(
                name: "UsersUsername",
                table: "category_user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_category_user",
                table: "category_user",
                columns: new[] { "category_key", "UsersUsername" });

            migrationBuilder.CreateIndex(
                name: "IX_category_user_UsersUsername",
                table: "category_user",
                column: "UsersUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_category_user_users_UsersUsername",
                table: "category_user",
                column: "UsersUsername",
                principalTable: "users",
                principalColumn: "username",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
