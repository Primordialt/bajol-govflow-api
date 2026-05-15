using Bajol.GovFlow.Application.Abstractions.Persistence;
using Bajol.GovFlow.Domain.Common;
using Bajol.GovFlow.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Bajol.GovFlow.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Applies creation and update audit metadata, normalizes user emails,
/// converts hard deletes on <see cref="AuditableEntity"/> instances into soft deletes,
/// and removes join assignments when identity aggregates are soft-deleted.
/// </summary>
public sealed class AuditableEntitySaveChangesInterceptor(
    IAuditUserProvider auditUserProvider,
    TimeProvider timeProvider) : SaveChangesInterceptor
{
    /// <inheritdoc />
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
        {
            ApplyAuditing(eventData.Context);
        }

        return base.SavingChanges(eventData, result);
    }

    /// <inheritdoc />
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            ApplyAuditing(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyAuditing(DbContext context)
    {
        var utcNow = timeProvider.GetUtcNow().UtcDateTime;
        var currentUserId = auditUserProvider.GetCurrentUserId();

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity.CreatedAt == default)
                    {
                        entry.Property(nameof(AuditableEntity.CreatedAt)).CurrentValue = utcNow;
                        entry.Property(nameof(AuditableEntity.CreatedBy)).CurrentValue = currentUserId;
                    }

                    NormalizeUserEmail(entry);
                    break;

                case EntityState.Modified:
                    entry.Property(nameof(AuditableEntity.UpdatedAt)).CurrentValue = utcNow;
                    entry.Property(nameof(AuditableEntity.UpdatedBy)).CurrentValue = currentUserId;

                    NormalizeUserEmail(entry);

                    if (WasSoftDeleted(entry))
                    {
                        SoftDeleteRelationshipCleanup.Apply(context, entry.Entity);
                    }

                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.SoftDelete(currentUserId, utcNow);
                    SoftDeleteRelationshipCleanup.Apply(context, entry.Entity);
                    break;
            }
        }
    }

    private static bool WasSoftDeleted(EntityEntry<AuditableEntity> entry) =>
        entry.Property(nameof(AuditableEntity.IsDeleted)).IsModified && entry.Entity.IsDeleted;

    private static void NormalizeUserEmail(EntityEntry<AuditableEntity> entry)
    {
        if (entry.Entity is not User)
        {
            return;
        }

        var emailProperty = entry.Property(nameof(User.Email));
        if (emailProperty.CurrentValue is string email &&
            (entry.State == EntityState.Added || emailProperty.IsModified))
        {
            emailProperty.CurrentValue = UserEmailNormalizer.Normalize(email);
        }
    }
}
