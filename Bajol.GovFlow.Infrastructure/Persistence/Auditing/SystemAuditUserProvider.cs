using Bajol.GovFlow.Application.Abstractions.Persistence;

namespace Bajol.GovFlow.Infrastructure.Persistence.Auditing;

/// <summary>
/// Fallback audit principal used when no interactive user context is available.
/// </summary>
public sealed class SystemAuditUserProvider : IAuditUserProvider
{
    /// <summary>
    /// The default principal name for background and design-time operations.
    /// </summary>
    public const string SystemPrincipal = "system";

    /// <inheritdoc />
    public string GetCurrentUserId() => SystemPrincipal;
}
