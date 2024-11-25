using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hydra.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniversityIdinDeletedUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "deleted_user_university_id",
                table: "deleted_user",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted_user_university_id",
                table: "deleted_user");
        }
    }
}
