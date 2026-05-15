using Bajol.GovFlow.Domain.Common;

namespace Bajol.GovFlow.Domain.Identity;

/// <summary>
/// Represents an application user account and its authentication profile.
/// </summary>
public sealed class User : AuditableEntity
{
    private User()
    {
    }

    /// <summary>
    /// Gets the user's display name.
    /// </summary>
    public string FullName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the unique email address used for sign-in and notifications.
    /// </summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the optional contact phone number in E.164 or regional format as captured by the application.
    /// </summary>
    public string? PhoneNumber { get; private set; }

    /// <summary>
    /// Gets the salted password hash. Never persist or log clear-text passwords.
    /// </summary>
    public string PasswordHash { get; private set; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether the account may authenticate.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets the UTC timestamp of the last successful authentication, if any.
    /// </summary>
    public DateTime? LastLoginAt { get; private set; }

    /// <summary>
    /// Gets the role assignments for this user.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

    /// <summary>
    /// Creates a new <see cref="User"/> with initial audit metadata.
    /// </summary>
    /// <param name="fullName">The user's display name.</param>
    /// <param name="email">The user's email address.</param>
    /// <param name="phoneNumber">An optional phone number.</param>
    /// <param name="passwordHash">The salted password hash.</param>
    /// <param name="createdBy">The creating principal or process.</param>
    /// <param name="createdAtUtc">The creation instant in UTC.</param>
    /// <returns>The new user instance.</returns>
    public static User Create(
        string fullName,
        string email,
        string? phoneNumber,
        string passwordHash,
        string createdBy,
        DateTime createdAtUtc)
    {
        var entity = new User
        {
            Id = Guid.NewGuid(),
            FullName = fullName,
            Email = email,
            PhoneNumber = phoneNumber,
            PasswordHash = passwordHash,
            IsActive = true
        };

        entity.ApplyCreatedAudit(createdBy, createdAtUtc);
        return entity;
    }

    /// <summary>
    /// Assigns a role to the user if that assignment does not already exist.
    /// </summary>
    /// <param name="roleId">The role identifier.</param>
    /// <returns><see langword="true"/> if a new assignment was added; otherwise <see langword="false"/>.</returns>
    public bool AssignRole(Guid roleId)
    {
        if (UserRoles.Any(x => x.RoleId == roleId))
        {
            return false;
        }

        UserRoles.Add(UserRole.Create(Id, roleId));
        return true;
    }

    /// <summary>
    /// Removes a role assignment from the user, if present.
    /// </summary>
    /// <param name="roleId">The role identifier.</param>
    /// <returns><see langword="true"/> if an assignment was removed; otherwise <see langword="false"/>.</returns>
    public bool RemoveRole(Guid roleId)
    {
        var assignment = UserRoles.FirstOrDefault(x => x.RoleId == roleId);
        if (assignment is null)
        {
            return false;
        }

        return UserRoles.Remove(assignment);
    }

    /// <summary>
    /// Records a successful authentication event.
    /// </summary>
    /// <param name="authenticatedAtUtc">The authentication instant in UTC.</param>
    /// <param name="updatedBy">The principal associated with this update (typically the authenticated user subject).</param>
    public void RecordSuccessfulLogin(DateTime authenticatedAtUtc, string updatedBy)
    {
        LastLoginAt = authenticatedAtUtc;
        ApplyUpdatedAudit(updatedBy, authenticatedAtUtc);
    }

    /// <summary>
    /// Updates the password hash after a credential rotation.
    /// </summary>
    /// <param name="passwordHash">The new salted password hash.</param>
    /// <param name="updatedBy">The updating principal or process.</param>
    /// <param name="updatedAtUtc">The update instant in UTC.</param>
    public void ChangePasswordHash(string passwordHash, string? updatedBy, DateTime updatedAtUtc)
    {
        PasswordHash = passwordHash;
        ApplyUpdatedAudit(updatedBy, updatedAtUtc);
    }

    /// <summary>
    /// Activates or deactivates the account for authentication purposes.
    /// </summary>
    /// <param name="isActive">Whether the account should be active.</param>
    /// <param name="updatedBy">The updating principal or process.</param>
    /// <param name="updatedAtUtc">The update instant in UTC.</param>
    public void SetActive(bool isActive, string? updatedBy, DateTime updatedAtUtc)
    {
        IsActive = isActive;
        ApplyUpdatedAudit(updatedBy, updatedAtUtc);
    }

    /// <summary>
    /// Updates profile fields that are safe to change without credential rotation.
    /// </summary>
    /// <param name="fullName">The user's display name.</param>
    /// <param name="email">The user's email address.</param>
    /// <param name="phoneNumber">An optional phone number.</param>
    /// <param name="updatedBy">The updating principal or process.</param>
    /// <param name="updatedAtUtc">The update instant in UTC.</param>
    public void UpdateProfile(string fullName, string email, string? phoneNumber, string? updatedBy, DateTime updatedAtUtc)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        ApplyUpdatedAudit(updatedBy, updatedAtUtc);
    }
}
