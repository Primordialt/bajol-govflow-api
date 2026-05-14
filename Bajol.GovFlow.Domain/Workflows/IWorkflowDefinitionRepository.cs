namespace Bajol.GovFlow.Domain.Workflows;

public interface IWorkflowDefinitionRepository
{
    Task<WorkflowDefinition?> GetPublishedAsync(string key, CancellationToken cancellationToken = default);
}
