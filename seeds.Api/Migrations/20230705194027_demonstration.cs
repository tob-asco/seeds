using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class demonstration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "users",
                newName: "ideee");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ideee",
                table: "users",
                newName: "id");
        }
    }
}
