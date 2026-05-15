using Bajol.GovFlow.Domain.Common;

namespace Bajol.GovFlow.Domain.Identity;

/// <summary>
/// Represents the many-to-many association between <see cref="User"/> and <see cref="Role"/>.
/// </summary>
public sealed class UserRole : BaseEntity
{
    private UserRole()
    {
    }

    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the role identifier.
    /// </summary>
    public Guid RoleId { get; private set; }

    /// <summary>
    /// Gets the user navigation.
    /// </summary>
    public User User { get; private set; } = null!;

    /// <summary>
    /// Gets the role navigation.
    /// </summary>
    public Role Role { get; private set; } = null!;

    /// <summary>
    /// Creates a new <see cref="UserRole"/> association.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="roleId">The role identifier.</param>
    /// <returns>The new association instance.</returns>
    public static UserRole Create(Guid userId, Guid roleId)
    {
        return new UserRole
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            RoleId = roleId
        };
    }
}
