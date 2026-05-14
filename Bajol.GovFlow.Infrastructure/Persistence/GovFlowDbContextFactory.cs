using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bajol.GovFlow.Infrastructure.Persistence;

public sealed class GovFlowDbContextFactory : IDesignTimeDbContextFactory<GovFlowDbContext>
{
    public GovFlowDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("GOVFLOW_DESIGN_POSTGRESQL")
            ?? "Host=localhost;Port=5432;Database=govflow;Username=govflow;Password=govflow";

        var optionsBuilder = new DbContextOptionsBuilder<GovFlowDbContext>();
        optionsBuilder.UseNpgsql(connectionString, npgsql =>
        {
            npgsql.MigrationsAssembly(typeof(GovFlowDbContext).Assembly.FullName);
        });

        return new GovFlowDbContext(optionsBuilder.Options);
    }
}
