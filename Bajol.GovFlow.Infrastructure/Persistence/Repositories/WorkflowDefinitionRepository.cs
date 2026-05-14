using Bajol.GovFlow.Domain.Workflows;
using Microsoft.EntityFrameworkCore;

namespace Bajol.GovFlow.Infrastructure.Persistence.Repositories;

public sealed class WorkflowDefinitionRepository(GovFlowDbContext dbContext) : IWorkflowDefinitionRepository
{
    public async Task<WorkflowDefinition?> GetPublishedAsync(string key, CancellationToken cancellationToken = default) =>
        await dbContext.WorkflowDefinitions
            .AsNoTracking()
            .Where(x => x.Key == key && x.IsPublished)
            .OrderByDescending(x => x.Version)
            .FirstOrDefaultAsync(cancellationToken);
}
