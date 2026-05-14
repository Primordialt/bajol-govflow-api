namespace Bajol.GovFlow.Domain.Workflows;

/// <summary>
/// Provides workflow definition payloads to orchestration components without embedding workflow rules in handlers.
/// </summary>
public interface IWorkflowDefinitionProvider
{
    Task<WorkflowDefinitionDocument?> GetPublishedDefinitionAsync(string key, CancellationToken cancellationToken = default);
}

public sealed record WorkflowDefinitionDocument(string Key, int Version, string Json);
