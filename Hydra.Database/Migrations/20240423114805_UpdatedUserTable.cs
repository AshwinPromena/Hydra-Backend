using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hydra.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_badge_badge_sequence_sequence_id",
                table: "badge");

            migrationBuilder.DropForeignKey(
                name: "FK_badge_user_user_id",
                table: "badge");

            migrationBuilder.DropForeignKey(
                name: "FK_badge_field_user_user_id",
                table: "badge_field");

            migrationBuilder.DropForeignKey(
                name: "FK_badge_media_user_user_id",
                table: "badge_media");

            migrationBuilder.DropIndex(
                name: "IX_badge_media_user_id",
                table: "badge_media");

            migrationBuilder.DropIndex(
                name: "IX_badge_field_user_id",
                table: "badge_field");

            migrationBuilder.DropIndex(
                name: "IX_badge_sequence_id",
                table: "badge");

            migrationBuilder.DropIndex(
                name: "IX_badge_user_id",
                table: "badge");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "badge_media");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "badge_media");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "badge_field");

            migrationBuilder.DropColumn(
                name: "sequence_id",
                table: "badge");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "badge");

            migrationBuilder.RenameColumn(
                name: "field_name",
                table: "badge_field",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "field_content",
                table: "badge_field",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "badge_name",
                table: "badge",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "badge_description",
                table: "badge",
                newName: "description");

            migrationBuilder.AddColumn<long>(
                name: "badge_sequence_id",
                table: "badge",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "badge",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "badge",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_date",
                table: "badge",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_badge_badge_sequence_id",
                table: "badge",
                column: "badge_sequence_id");

            migrationBuilder.AddForeignKey(
                name: "FK_badge_badge_sequence_badge_sequence_id",
                table: "badge",
                column: "badge_sequence_id",
                principalTable: "badge_sequence",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_badge_badge_sequence_badge_sequence_id",
                table: "badge");

            migrationBuilder.DropIndex(
                name: "IX_badge_badge_sequence_id",
                table: "badge");

            migrationBuilder.DropColumn(
                name: "badge_sequence_id",
                table: "badge");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "badge");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "badge");

            migrationBuilder.DropColumn(
                name: "updated_date",
                table: "badge");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "badge_field",
                newName: "field_name");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "badge_field",
                newName: "field_content");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "badge",
                newName: "badge_name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "badge",
                newName: "badge_description");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "badge_media",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "user_id",
                table: "badge_media",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "user_id",
                table: "badge_field",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "sequence_id",
                table: "badge",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "user_id",
                table: "badge",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_badge_media_user_id",
                table: "badge_media",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_badge_field_user_id",
                table: "badge_field",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_badge_sequence_id",
                table: "badge",
                column: "sequence_id");

            migrationBuilder.CreateIndex(
                name: "IX_badge_user_id",
                table: "badge",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_badge_badge_sequence_sequence_id",
                table: "badge",
                column: "sequence_id",
                principalTable: "badge_sequence",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_badge_user_user_id",
                table: "badge",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_badge_field_user_user_id",
                table: "badge_field",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_badge_media_user_user_id",
                table: "badge_media",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
