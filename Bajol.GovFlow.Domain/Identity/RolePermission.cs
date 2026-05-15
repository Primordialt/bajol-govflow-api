using Bajol.GovFlow.Domain.Common;

namespace Bajol.GovFlow.Domain.Identity;

/// <summary>
/// Represents the many-to-many association between <see cref="Role"/> and <see cref="Permission"/>.
/// </summary>
public sealed class RolePermission : BaseEntity
{
    private RolePermission()
    {
    }

    /// <summary>
    /// Gets the role identifier.
    /// </summary>
    public Guid RoleId { get; private set; }

    /// <summary>
    /// Gets the permission identifier.
    /// </summary>
    public Guid PermissionId { get; private set; }

    /// <summary>
    /// Gets the role navigation.
    /// </summary>
    public Role Role { get; private set; } = null!;

    /// <summary>
    /// Gets the permission navigation.
    /// </summary>
    public Permission Permission { get; private set; } = null!;

    /// <summary>
    /// Creates a new <see cref="RolePermission"/> association.
    /// </summary>
    /// <param name="roleId">The role identifier.</param>
    /// <param name="permissionId">The permission identifier.</param>
    /// <returns>The new association instance.</returns>
    public static RolePermission Create(Guid roleId, Guid permissionId)
    {
        return new RolePermission
        {
            Id = Guid.NewGuid(),
            RoleId = roleId,
            PermissionId = permissionId
        };
    }
}
