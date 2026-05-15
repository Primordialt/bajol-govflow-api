using Bajol.GovFlow.Domain.Correspondences;
using Bajol.GovFlow.Domain.Identity;
using Bajol.GovFlow.Domain.UnitOfWork;
using Bajol.GovFlow.Domain.Workflows;
using Microsoft.EntityFrameworkCore;

namespace Bajol.GovFlow.Infrastructure.Persistence;

/// <summary>
/// Primary Entity Framework Core database context for the GovFlow platform.
/// </summary>
/// <remarks>
/// Entity mappings are applied from the Infrastructure assembly via <see cref="IEntityTypeConfiguration{TEntity}"/>
/// implementations. Soft-delete query filters and audit metadata for identity aggregates are configured per entity;
/// <see cref="Interceptors.AuditableEntitySaveChangesInterceptor"/> enforces audit and soft-delete semantics on save.
/// </remarks>
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    /// <summary>
    /// Application user accounts.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Authorization roles.
    /// </summary>
    public DbSet<Role> Roles => Set<Role>();

    /// <summary>
    /// Discrete permissions assignable to roles.
    /// </summary>
    public DbSet<Permission> Permissions => Set<Permission>();

    /// <summary>
    /// User-to-role assignments (many-to-many join).
    /// </summary>
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    /// <summary>
    /// Role-to-permission assignments (many-to-many join).
    /// </summary>
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    /// <summary>
    /// Government correspondence records.
    /// </summary>
    public DbSet<Correspondence> Correspondences => Set<Correspondence>();

    /// <summary>
    /// Versioned workflow definitions.
    /// </summary>
    public DbSet<WorkflowDefinition> WorkflowDefinitions => Set<WorkflowDefinition>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    Task<int> IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken) =>
        base.SaveChangesAsync(cancellationToken);
}
