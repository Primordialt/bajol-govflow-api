namespace Bajol.GovFlow.Domain.Common;

/// <summary>
/// Base abstraction for entities that require creation and update audit metadata,
/// and that support logical deletion without physical removal from storage.
/// </summary>
/// <remarks>
/// All <see cref="DateTime"/> values are expressed in UTC.
/// </remarks>
public abstract class AuditableEntity : BaseEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuditableEntity"/> class.
    /// </summary>
    /// <remarks>
    /// Required by object-relational mappers for materialization from persistence.
    /// </remarks>
    protected AuditableEntity()
    {
    }

    /// <summary>
    /// Gets the UTC timestamp when the entity was first persisted.
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// Gets the principal or process that created the entity (for example a user subject identifier).
    /// </summary>
    public string CreatedBy { get; protected set; } = string.Empty;

    /// <summary>
    /// Gets the UTC timestamp of the most recent update, if any.
    /// </summary>
    public DateTime? UpdatedAt { get; protected set; }

    /// <summary>
    /// Gets the principal or process that performed the most recent update, if any.
    /// </summary>
    public string? UpdatedBy { get; protected set; }

    /// <summary>
    /// Gets a value indicating whether the entity has been logically deleted.
    /// </summary>
    public bool IsDeleted { get; protected set; }

    /// <summary>
    /// Gets the UTC timestamp of the logical deletion, if applicable.
    /// </summary>
    public DateTime? DeletedAt { get; protected set; }

    /// <summary>
    /// Gets the principal or process that performed the logical deletion, if applicable.
    /// </summary>
    public string? DeletedBy { get; protected set; }

    /// <summary>
    /// Records creation audit metadata. Intended for use from entity factories.
    /// </summary>
    /// <param name="createdBy">The creating principal or process.</param>
    /// <param name="createdAtUtc">The creation instant in UTC.</param>
    protected void ApplyCreatedAudit(string createdBy, DateTime createdAtUtc)
    {
        CreatedBy = createdBy;
        CreatedAt = createdAtUtc;
    }

    /// <summary>
    /// Records update audit metadata.
    /// </summary>
    /// <param name="updatedBy">The updating principal or process.</param>
    /// <param name="updatedAtUtc">The update instant in UTC.</param>
    protected void ApplyUpdatedAudit(string? updatedBy, DateTime updatedAtUtc)
    {
        UpdatedBy = updatedBy;
        UpdatedAt = updatedAtUtc;
    }

    /// <summary>
    /// Marks the entity as logically deleted and records deletion audit metadata.
    /// </summary>
    /// <param name="deletedBy">The deleting principal or process.</param>
    /// <param name="deletedAtUtc">The deletion instant in UTC.</param>
    public void SoftDelete(string deletedBy, DateTime deletedAtUtc)
    {
        if (IsDeleted)
        {
            return;
        }

        IsDeleted = true;
        DeletedBy = deletedBy;
        DeletedAt = deletedAtUtc;
        ApplyUpdatedAudit(deletedBy, deletedAtUtc);
    }

    /// <summary>
    /// Reverses a logical deletion for scenarios such as administrative restoration.
    /// </summary>
    /// <param name="restoredBy">The principal or process that performed the restoration.</param>
    /// <param name="restoredAtUtc">The restoration instant in UTC.</param>
    public void RestoreFromSoftDelete(string restoredBy, DateTime restoredAtUtc)
    {
        if (!IsDeleted)
        {
            return;
        }

        IsDeleted = false;
        DeletedBy = null;
        DeletedAt = null;
        ApplyUpdatedAudit(restoredBy, restoredAtUtc);
    }
}
