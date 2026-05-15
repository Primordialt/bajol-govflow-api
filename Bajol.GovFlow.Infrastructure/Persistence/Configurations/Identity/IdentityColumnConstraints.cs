namespace Bajol.GovFlow.Infrastructure.Persistence.Configurations.Identity;

/// <summary>
/// Centralized column length and PostgreSQL index names for identity mappings.
/// </summary>
internal static class IdentityColumnConstraints
{
    internal static class User
    {
        internal const int FullNameMaxLength = 256;
        internal const int EmailMaxLength = 320;
        internal const int PhoneNumberMaxLength = 32;
        internal const int PasswordHashMaxLength = 512;
        internal const string EmailUniqueIndex = "ux_users_email_not_deleted";
    }

    internal static class Role
    {
        internal const int NameMaxLength = 128;
        internal const int DescriptionMaxLength = 1024;
        internal const string NameUniqueIndex = "ux_roles_name_not_deleted";
    }

    internal static class Permission
    {
        internal const int NameMaxLength = 256;
        internal const int CodeMaxLength = 128;
        internal const int DescriptionMaxLength = 1024;
        internal const string CodeUniqueIndex = "ux_permissions_code_not_deleted";
    }

    internal static class Audit
    {
        internal const int PrincipalMaxLength = 256;
    }

    internal static class UserRole
    {
        internal const string UserRoleUniqueIndex = "ux_user_roles_user_id_role_id";
        internal const string RoleIdIndex = "ix_user_roles_role_id";
    }

    internal static class RolePermission
    {
        internal const string RolePermissionUniqueIndex = "ux_role_permissions_role_id_permission_id";
        internal const string PermissionIdIndex = "ix_role_permissions_permission_id";
    }

    internal const string PostgreSqlUuid = "uuid";
    internal const string PostgreSqlTimestamptz = "timestamp with time zone";
    internal const string PostgreSqlBoolean = "boolean";
}
