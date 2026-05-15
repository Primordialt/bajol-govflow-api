using Bajol.GovFlow.Application.Abstractions.Persistence;
using Bajol.GovFlow.Domain.UnitOfWork;
using Bajol.GovFlow.Infrastructure.Persistence.Auditing;
using Bajol.GovFlow.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bajol.GovFlow.Infrastructure.Persistence.Extensions;

/// <summary>
/// Registers PostgreSQL persistence services for the GovFlow platform.
/// </summary>
public static class PersistenceServiceCollectionExtensions
{
    /// <summary>
    /// Adds <see cref="AppDbContext"/> with PostgreSQL, auditing interceptors, and unit-of-work registration.
    /// </summary>
    public static IServiceCollection AddAppPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var postgresConnection = configuration.GetConnectionString("PostgreSQL")
            ?? throw new InvalidOperationException("Connection string 'PostgreSQL' is not configured.");

        services.AddHttpContextAccessor();
        services.AddScoped<IAuditUserProvider, HttpContextAuditUserProvider>();
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(postgresConnection, npgsql =>
                {
                    npgsql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    npgsql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                })
                .AddInterceptors(serviceProvider.GetRequiredService<AuditableEntitySaveChangesInterceptor>());
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        return services;
    }
}
