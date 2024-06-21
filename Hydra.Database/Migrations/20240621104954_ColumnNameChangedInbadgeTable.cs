using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hydra.Database.Migrations
{
    /// <inheritdoc />
    public partial class ColumnNameChangedInbadgeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_badge_user_approvalUserId",
                table: "badge");

            migrationBuilder.RenameColumn(
                name: "approvalUserId",
                table: "badge",
                newName: "approval_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_badge_approvalUserId",
                table: "badge",
                newName: "IX_badge_approval_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_badge_user_approval_user_id",
                table: "badge",
                column: "approval_user_id",
                principalTable: "user",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_badge_user_approval_user_id",
                table: "badge");

            migrationBuilder.RenameColumn(
                name: "approval_user_id",
                table: "badge",
                newName: "approvalUserId");

            migrationBuilder.RenameIndex(
                name: "IX_badge_approval_user_id",
                table: "badge",
                newName: "IX_badge_approvalUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_badge_user_approvalUserId",
                table: "badge",
                column: "approvalUserId",
                principalTable: "user",
                principalColumn: "id");
        }
    }
}
