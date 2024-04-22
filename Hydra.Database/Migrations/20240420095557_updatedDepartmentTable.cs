using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hydra.Database.Migrations
{
    /// <inheritdoc />
    public partial class updatedDepartmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_badge_sequence_user_user_id",
                table: "badge_sequence");

            migrationBuilder.DropIndex(
                name: "IX_badge_sequence_user_id",
                table: "badge_sequence");

            migrationBuilder.DropColumn(
                name: "sequence_name",
                table: "badge_sequence");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "badge_sequence");

            migrationBuilder.RenameColumn(
                name: "department_name",
                table: "department",
                newName: "name");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "badge_sequence",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "badge_sequence");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "department",
                newName: "department_name");

            migrationBuilder.AddColumn<string>(
                name: "sequence_name",
                table: "badge_sequence",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "user_id",
                table: "badge_sequence",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_badge_sequence_user_id",
                table: "badge_sequence",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_badge_sequence_user_user_id",
                table: "badge_sequence",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
