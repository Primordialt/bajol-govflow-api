using Bajol.GovFlow.Domain.Workflows;

namespace Bajol.GovFlow.Infrastructure.Workflows;

public sealed class DbWorkflowDefinitionProvider(IWorkflowDefinitionRepository repository) : IWorkflowDefinitionProvider
{
    public async Task<WorkflowDefinitionDocument?> GetPublishedDefinitionAsync(string key, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetPublishedAsync(key, cancellationToken);
        return entity is null
            ? null
            : new WorkflowDefinitionDocument(entity.Key, entity.Version, entity.DefinitionDocument);
    }
}
