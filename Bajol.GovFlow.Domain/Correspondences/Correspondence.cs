using Bajol.GovFlow.Domain.Common;

namespace Bajol.GovFlow.Domain.Correspondences;

public sealed class Correspondence : Entity<Guid>, IAuditableEntity
{
    private Correspondence()
    {
    }

    public string Subject { get; private set; } = string.Empty;
    public string ReferenceNumber { get; private set; } = string.Empty;
    public CorrespondenceStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public DateTime? ModifiedAtUtc { get; private set; }
    public string? ModifiedBy { get; private set; }

    public static Correspondence Create(
        string subject,
        string referenceNumber,
        string createdBy,
        DateTime utcNow)
    {
        return new Correspondence
        {
            Id = Guid.NewGuid(),
            Subject = subject,
            ReferenceNumber = referenceNumber,
            Status = CorrespondenceStatus.Draft,
            CreatedAtUtc = utcNow,
            CreatedBy = createdBy
        };
    }

    public void Submit(string modifiedBy, DateTime utcNow)
    {
        Status = CorrespondenceStatus.Submitted;
        ModifiedAtUtc = utcNow;
        ModifiedBy = modifiedBy;
    }
}
