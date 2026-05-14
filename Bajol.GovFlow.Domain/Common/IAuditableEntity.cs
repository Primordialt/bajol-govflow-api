namespace Bajol.GovFlow.Domain.Common;

public interface IAuditableEntity
{
    DateTime CreatedAtUtc { get; }
    string CreatedBy { get; }
    DateTime? ModifiedAtUtc { get; }
    string? ModifiedBy { get; }
}
