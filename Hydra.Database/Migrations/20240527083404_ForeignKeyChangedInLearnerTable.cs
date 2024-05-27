using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hydra.Database.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyChangedInLearnerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_deleted_learner_user_user_id",
                table: "deleted_learner");

            migrationBuilder.DropIndex(
                name: "IX_deleted_learner_user_id",
                table: "deleted_learner");

            migrationBuilder.CreateIndex(
                name: "IX_deleted_learner_learner_id",
                table: "deleted_learner",
                column: "learner_id");

            migrationBuilder.AddForeignKey(
                name: "FK_deleted_learner_user_learner_id",
                table: "deleted_learner",
                column: "learner_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_deleted_learner_user_learner_id",
                table: "deleted_learner");

            migrationBuilder.DropIndex(
                name: "IX_deleted_learner_learner_id",
                table: "deleted_learner");

            migrationBuilder.CreateIndex(
                name: "IX_deleted_learner_user_id",
                table: "deleted_learner",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_deleted_learner_user_user_id",
                table: "deleted_learner",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
