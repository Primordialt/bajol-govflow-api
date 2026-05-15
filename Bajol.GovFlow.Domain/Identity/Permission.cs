using Bajol.GovFlow.Domain.Common;

namespace Bajol.GovFlow.Domain.Identity;

/// <summary>
/// Represents a discrete authorization permission that can be attached to roles.
/// </summary>
public sealed class Permission : AuditableEntity
{
    private Permission()
    {
    }

    /// <summary>
    /// Gets the human-readable permission name.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the stable machine-oriented permission code used by policies and integration points.
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>
    /// Gets an optional description of what the permission authorizes.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the roles that include this permission.
    /// </summary>
    public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();

    /// <summary>
    /// Creates a new <see cref="Permission"/> with initial audit metadata.
    /// </summary>
    /// <param name="name">The permission name.</param>
    /// <param name="code">The stable permission code.</param>
    /// <param name="description">An optional description.</param>
    /// <param name="createdBy">The creating principal or process.</param>
    /// <param name="createdAtUtc">The creation instant in UTC.</param>
    /// <returns>The new permission instance.</returns>
    public static Permission Create(
        string name,
        string code,
        string? description,
        string createdBy,
        DateTime createdAtUtc)
    {
        var entity = new Permission
        {
            Id = Guid.NewGuid(),
            Name = name,
            Code = code,
            Description = description
        };

        entity.ApplyCreatedAudit(createdBy, createdAtUtc);
        return entity;
    }

    /// <summary>
    /// Updates the mutable permission fields.
    /// </summary>
    /// <param name="name">The permission name.</param>
    /// <param name="code">The stable permission code.</param>
    /// <param name="description">An optional description.</param>
    /// <param name="updatedBy">The updating principal or process.</param>
    /// <param name="updatedAtUtc">The update instant in UTC.</param>
    public void Update(string name, string code, string? description, string? updatedBy, DateTime updatedAtUtc)
    {
        Name = name;
        Code = code;
        Description = description;
        ApplyUpdatedAudit(updatedBy, updatedAtUtc);
    }
}
