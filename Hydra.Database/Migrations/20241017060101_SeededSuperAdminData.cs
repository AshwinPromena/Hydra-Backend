using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hydra.Database.Migrations
{
    /// <inheritdoc />
    public partial class SeededSuperAdminData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "id", "name" },
                values: new object[] { 5L, "Super Admin" });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "id", "access_level_id", "created_date", "department_id", "email", "email_2", "email_3", "first_name", "is_active", "is_approved", "is_archived", "last_name", "learner_id", "mobile_number", "password", "profile_picture", "university_id", "updated_date", "user_name" },
                values: new object[] { 2L, 3L, new DateTime(2024, 4, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "superadmin@yopmail.com", null, null, "SuperAdmin", true, true, false, "", null, null, "3AhCUZedQxVLajDQSZhRirNTvEyK/luGud/X7oAXJX0=", null, 0L, new DateTime(2024, 4, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "user_role",
                columns: new[] { "id", "role_id", "user_id" },
                values: new object[] { 224L, 5L, 2L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "user_role",
                keyColumn: "id",
                keyValue: 224L);

            migrationBuilder.DeleteData(
                table: "role",
                keyColumn: "id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "user",
                keyColumn: "id",
                keyValue: 2L);
        }
    }
}
