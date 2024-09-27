using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hydra.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniversityIdInUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "university_id",
                table: "user",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "id",
                keyValue: 1L,
                column: "university_id",
                value: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "university_id",
                table: "user");
        }
    }
}
