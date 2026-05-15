namespace Bajol.GovFlow.Domain.Common;

/// <summary>
/// Base abstraction for all domain entities that are keyed by a surrogate <see cref="Guid"/> identifier.
/// </summary>
public abstract class BaseEntity : Entity<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEntity"/> class.
    /// </summary>
    /// <remarks>
    /// Required by object-relational mappers for materialization from persistence.
    /// </remarks>
    protected BaseEntity()
    {
    }
}
