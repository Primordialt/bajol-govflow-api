using Bajol.GovFlow.Domain.Correspondences;
using Bajol.GovFlow.Domain.UnitOfWork;
using Bajol.GovFlow.Domain.Workflows;
using Microsoft.EntityFrameworkCore;

namespace Bajol.GovFlow.Infrastructure.Persistence;

public sealed class GovFlowDbContext(DbContextOptions<GovFlowDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Correspondence> Correspondences => Set<Correspondence>();
    public DbSet<WorkflowDefinition> WorkflowDefinitions => Set<WorkflowDefinition>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GovFlowDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
