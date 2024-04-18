using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hydra.Database.Migrations
{
    /// <inheritdoc />
    public partial class secondmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_badge_approval_user_id",
                table: "badge_approval",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_badge_department_id",
                table: "badge",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_badge_sequence_id",
                table: "badge",
                column: "sequence_id");

            migrationBuilder.AddForeignKey(
                name: "FK_badge_badge_sequence_sequence_id",
                table: "badge",
                column: "sequence_id",
                principalTable: "badge_sequence",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_badge_department_department_id",
                table: "badge",
                column: "department_id",
                principalTable: "department",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_badge_approval_user_user_id",
                table: "badge_approval",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_badge_badge_sequence_sequence_id",
                table: "badge");

            migrationBuilder.DropForeignKey(
                name: "FK_badge_department_department_id",
                table: "badge");

            migrationBuilder.DropForeignKey(
                name: "FK_badge_approval_user_user_id",
                table: "badge_approval");

            migrationBuilder.DropIndex(
                name: "IX_badge_approval_user_id",
                table: "badge_approval");

            migrationBuilder.DropIndex(
                name: "IX_badge_department_id",
                table: "badge");

            migrationBuilder.DropIndex(
                name: "IX_badge_sequence_id",
                table: "badge");
        }
    }
}
