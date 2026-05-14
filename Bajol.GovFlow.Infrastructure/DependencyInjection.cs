using Bajol.GovFlow.Application.Search;
using Bajol.GovFlow.Domain.Repositories;
using Bajol.GovFlow.Domain.UnitOfWork;
using Bajol.GovFlow.Domain.Workflows;
using Bajol.GovFlow.Infrastructure.Persistence;
using Bajol.GovFlow.Infrastructure.Persistence.Repositories;
using Bajol.GovFlow.Infrastructure.Search;
using Bajol.GovFlow.Infrastructure.Security;
using Bajol.GovFlow.Infrastructure.Workflows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bajol.GovFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var postgresConnection = configuration.GetConnectionString("PostgreSQL")
            ?? throw new InvalidOperationException("Connection string 'PostgreSQL' is not configured.");

        services.AddDbContext<GovFlowDbContext>(options =>
            options.UseNpgsql(postgresConnection, npgsql =>
            {
                npgsql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                npgsql.MigrationsAssembly(typeof(GovFlowDbContext).Assembly.FullName);
            }));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<GovFlowDbContext>());
        services.AddScoped<ICorrespondenceRepository, CorrespondenceRepository>();
        services.AddScoped<IWorkflowDefinitionRepository, WorkflowDefinitionRepository>();
        services.AddScoped<IWorkflowDefinitionProvider, DbWorkflowDefinitionProvider>();

        services.AddGovFlowJwtAuthentication(configuration.GetSection(JwtSettings.SectionName));

        var redisConnection = configuration.GetConnectionString("Redis");
        if (!string.IsNullOrWhiteSpace(redisConnection))
        {
            services.AddStackExchangeRedisCache(options => { options.Configuration = redisConnection; });
        }

        services.Configure<ElasticsearchOptions>(configuration.GetSection(ElasticsearchOptions.SectionName));
        services.AddSingleton<ISearchGateway, ElasticsearchSearchGateway>();

        services.AddSingleton<TimeProvider>(TimeProvider.System);

        return services;
    }
}
