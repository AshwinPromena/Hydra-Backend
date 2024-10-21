using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hydra.Database.Migrations
{
    /// <inheritdoc />
    public partial class SeededdataforlogintypeinRoletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "id",
                keyValue: 1L,
                column: "login_type",
                value: 2);

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "id",
                keyValue: 2L,
                column: "login_type",
                value: 2);

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "id",
                keyValue: 3L,
                column: "login_type",
                value: 2);

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "id",
                keyValue: 4L,
                column: "login_type",
                value: 2);

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "id",
                keyValue: 5L,
                column: "login_type",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "id",
                keyValue: 1L,
                column: "login_type",
                value: 0);

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "id",
                keyValue: 2L,
                column: "login_type",
                value: 0);

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "id",
                keyValue: 3L,
                column: "login_type",
                value: 0);

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "id",
                keyValue: 4L,
                column: "login_type",
                value: 0);

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "id",
                keyValue: 5L,
                column: "login_type",
                value: 0);
        }
    }
}
