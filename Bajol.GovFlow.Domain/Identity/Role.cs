using Bajol.GovFlow.Domain.Common;

namespace Bajol.GovFlow.Domain.Identity;

/// <summary>
/// Represents a named role that can be granted to users and associated with permissions.
/// </summary>
public sealed class Role : AuditableEntity
{
    private Role()
    {
    }

    /// <summary>
    /// Gets the human-readable role name.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets an optional description of the role's purpose.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the users that have been assigned this role.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

    /// <summary>
    /// Gets the permissions linked to this role.
    /// </summary>
    public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();

    /// <summary>
    /// Creates a new <see cref="Role"/> with initial audit metadata.
    /// </summary>
    /// <param name="name">The role name.</param>
    /// <param name="description">An optional description.</param>
    /// <param name="createdBy">The creating principal or process.</param>
    /// <param name="createdAtUtc">The creation instant in UTC.</param>
    /// <returns>The new role instance.</returns>
    public static Role Create(string name, string? description, string createdBy, DateTime createdAtUtc)
    {
        var entity = new Role
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description
        };

        entity.ApplyCreatedAudit(createdBy, createdAtUtc);
        return entity;
    }

    /// <summary>
    /// Updates the mutable role fields.
    /// </summary>
    /// <param name="name">The role name.</param>
    /// <param name="description">An optional description.</param>
    /// <param name="updatedBy">The updating principal or process.</param>
    /// <param name="updatedAtUtc">The update instant in UTC.</param>
    public void Update(string name, string? description, string? updatedBy, DateTime updatedAtUtc)
    {
        Name = name;
        Description = description;
        ApplyUpdatedAudit(updatedBy, updatedAtUtc);
    }

    /// <summary>
    /// Links a permission to the role if that link does not already exist.
    /// </summary>
    /// <param name="permissionId">The permission identifier.</param>
    /// <returns><see langword="true"/> if a new link was added; otherwise <see langword="false"/>.</returns>
    public bool GrantPermission(Guid permissionId)
    {
        if (RolePermissions.Any(x => x.PermissionId == permissionId))
        {
            return false;
        }

        RolePermissions.Add(RolePermission.Create(Id, permissionId));
        return true;
    }

    /// <summary>
    /// Removes a permission link from the role, if present.
    /// </summary>
    /// <param name="permissionId">The permission identifier.</param>
    /// <returns><see langword="true"/> if a link was removed; otherwise <see langword="false"/>.</returns>
    public bool RevokePermission(Guid permissionId)
    {
        var link = RolePermissions.FirstOrDefault(x => x.PermissionId == permissionId);
        if (link is null)
        {
            return false;
        }

        return RolePermissions.Remove(link);
    }
}
