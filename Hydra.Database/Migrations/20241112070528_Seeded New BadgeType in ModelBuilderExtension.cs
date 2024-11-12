using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hydra.Database.Migrations
{
    /// <inheritdoc />
    public partial class SeededNewBadgeTypeinModelBuilderExtension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "badge_type",
                columns: new[] { "id", "name" },
                values: new object[] { 5L, "Marksheet" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "badge_type",
                keyColumn: "id",
                keyValue: 5L);
        }
    }
}
