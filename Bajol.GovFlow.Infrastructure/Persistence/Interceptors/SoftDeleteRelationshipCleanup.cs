using Bajol.GovFlow.Domain.Common;
using Bajol.GovFlow.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bajol.GovFlow.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Removes many-to-many join rows when identity aggregates are soft-deleted.
/// </summary>
internal static class SoftDeleteRelationshipCleanup
{
    public static void Apply(DbContext context, AuditableEntity entity)
    {
        switch (entity)
        {
            case User user:
                RemoveUserRoles(context, user.Id);
                user.UserRoles.Clear();
                break;

            case Role role:
                RemoveUserRolesForRole(context, role.Id);
                RemoveRolePermissionsForRole(context, role.Id);
                role.UserRoles.Clear();
                role.RolePermissions.Clear();
                break;

            case Permission permission:
                RemoveRolePermissionsForPermission(context, permission.Id);
                permission.RolePermissions.Clear();
                break;
        }
    }

    private static void RemoveUserRoles(DbContext context, Guid userId)
    {
        var assignments = context.Set<UserRole>().Where(x => x.UserId == userId).ToList();
        context.Set<UserRole>().RemoveRange(assignments);
    }

    private static void RemoveUserRolesForRole(DbContext context, Guid roleId)
    {
        var assignments = context.Set<UserRole>().Where(x => x.RoleId == roleId).ToList();
        context.Set<UserRole>().RemoveRange(assignments);
    }

    private static void RemoveRolePermissionsForRole(DbContext context, Guid roleId)
    {
        var assignments = context.Set<RolePermission>().Where(x => x.RoleId == roleId).ToList();
        context.Set<RolePermission>().RemoveRange(assignments);
    }

    private static void RemoveRolePermissionsForPermission(DbContext context, Guid permissionId)
    {
        var assignments = context.Set<RolePermission>().Where(x => x.PermissionId == permissionId).ToList();
        context.Set<RolePermission>().RemoveRange(assignments);
    }
}
