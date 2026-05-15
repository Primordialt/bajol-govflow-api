using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bajol.GovFlow.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IdentityEfConfigurationHardening : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_users_Email",
                table: "users",
                newName: "ux_users_email_not_deleted");

            migrationBuilder.RenameIndex(
                name: "IX_user_roles_UserId_RoleId",
                table: "user_roles",
                newName: "ux_user_roles_user_id_role_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_roles_RoleId",
                table: "user_roles",
                newName: "ix_user_roles_role_id");

            migrationBuilder.RenameIndex(
                name: "IX_roles_Name",
                table: "roles",
                newName: "ux_roles_name_not_deleted");

            migrationBuilder.RenameIndex(
                name: "IX_role_permissions_RoleId_PermissionId",
                table: "role_permissions",
                newName: "ux_role_permissions_role_id_permission_id");

            migrationBuilder.RenameIndex(
                name: "IX_role_permissions_PermissionId",
                table: "role_permissions",
                newName: "ix_role_permissions_permission_id");

            migrationBuilder.RenameIndex(
                name: "IX_permissions_Code",
                table: "permissions",
                newName: "ux_permissions_code_not_deleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "ux_users_email_not_deleted",
                table: "users",
                newName: "IX_users_Email");

            migrationBuilder.RenameIndex(
                name: "ux_user_roles_user_id_role_id",
                table: "user_roles",
                newName: "IX_user_roles_UserId_RoleId");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_role_id",
                table: "user_roles",
                newName: "IX_user_roles_RoleId");

            migrationBuilder.RenameIndex(
                name: "ux_roles_name_not_deleted",
                table: "roles",
                newName: "IX_roles_Name");

            migrationBuilder.RenameIndex(
                name: "ux_role_permissions_role_id_permission_id",
                table: "role_permissions",
                newName: "IX_role_permissions_RoleId_PermissionId");

            migrationBuilder.RenameIndex(
                name: "ix_role_permissions_permission_id",
                table: "role_permissions",
                newName: "IX_role_permissions_PermissionId");

            migrationBuilder.RenameIndex(
                name: "ux_permissions_code_not_deleted",
                table: "permissions",
                newName: "IX_permissions_Code");
        }
    }
}
