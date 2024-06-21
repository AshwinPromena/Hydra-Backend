using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hydra.Database.Migrations
{
    /// <inheritdoc />
    public partial class CreatedNewTableBadgeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "badge_type_id",
                table: "badge",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "badge_type",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_badge_type", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "badge_type",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1L, "Badge" },
                    { 2L, "Certificate" },
                    { 3L, "License" },
                    { 4L, "Miscellaneous" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_badge_badge_type_id",
                table: "badge",
                column: "badge_type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_badge_badge_type_badge_type_id",
                table: "badge",
                column: "badge_type_id",
                principalTable: "badge_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_badge_badge_type_badge_type_id",
                table: "badge");

            migrationBuilder.DropTable(
                name: "badge_type");

            migrationBuilder.DropIndex(
                name: "IX_badge_badge_type_id",
                table: "badge");

            migrationBuilder.DropColumn(
                name: "badge_type_id",
                table: "badge");
        }
    }
}
