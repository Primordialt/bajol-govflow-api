using Bajol.GovFlow.Domain.Correspondences;
using Bajol.GovFlow.Domain.Identity;
using Bajol.GovFlow.Domain.UnitOfWork;
using Bajol.GovFlow.Domain.Workflows;
using Microsoft.EntityFrameworkCore;

namespace Bajol.GovFlow.Infrastructure.Persistence;

public sealed class GovFlowDbContext(DbContextOptions<GovFlowDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Correspondence> Correspondences => Set<Correspondence>();
    public DbSet<WorkflowDefinition> WorkflowDefinitions => Set<WorkflowDefinition>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GovFlowDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
