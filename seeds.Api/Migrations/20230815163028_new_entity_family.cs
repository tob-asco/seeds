using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seeds.Api.Migrations
{
    /// <inheritdoc />
    public partial class new_entity_family : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "family_id",
                table: "topics",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "families",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    category_key = table.Column<string>(type: "character varying(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_families", x => x.id);
                    table.ForeignKey(
                        name: "FK_families_categories_category_key",
                        column: x => x.category_key,
                        principalTable: "categories",
                        principalColumn: "key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_topics_family_id",
                table: "topics",
                column: "family_id");

            migrationBuilder.CreateIndex(
                name: "IX_families_category_key",
                table: "families",
                column: "category_key");

            migrationBuilder.AddForeignKey(
                name: "FK_topics_families_family_id",
                table: "topics",
                column: "family_id",
                principalTable: "families",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_topics_families_family_id",
                table: "topics");

            migrationBuilder.DropTable(
                name: "families");

            migrationBuilder.DropIndex(
                name: "IX_topics_family_id",
                table: "topics");

            migrationBuilder.DropColumn(
                name: "family_id",
                table: "topics");
        }
    }
}
