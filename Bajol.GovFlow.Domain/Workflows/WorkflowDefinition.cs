using Bajol.GovFlow.Domain.Common;

namespace Bajol.GovFlow.Domain.Workflows;

/// <summary>
/// Persisted workflow definition. Transitions and steps are interpreted from <see cref="DefinitionDocument"/>
/// by infrastructure services, not by branching logic in application handlers.
/// </summary>
public sealed class WorkflowDefinition : Entity<Guid>
{
    private WorkflowDefinition()
    {
    }

    public string Key { get; private set; } = string.Empty;
    public int Version { get; private set; }
    public string DefinitionDocument { get; private set; } = "{}";
    public bool IsPublished { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public static WorkflowDefinition Create(string key, int version, string definitionDocument, bool isPublished, DateTime utcNow)
    {
        return new WorkflowDefinition
        {
            Id = Guid.NewGuid(),
            Key = key,
            Version = version,
            DefinitionDocument = definitionDocument,
            IsPublished = isPublished,
            CreatedAtUtc = utcNow
        };
    }
}
