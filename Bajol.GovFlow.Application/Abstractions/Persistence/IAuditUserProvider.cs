namespace Bajol.GovFlow.Application.Abstractions.Persistence;

/// <summary>
/// Resolves the current principal identifier used for persistence audit columns.
/// </summary>
public interface IAuditUserProvider
{
    /// <summary>
    /// Gets the identifier of the user or process performing the current operation.
    /// </summary>
    string GetCurrentUserId();
}
