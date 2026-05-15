using System.Security.Claims;
using Bajol.GovFlow.Application.Abstractions.Persistence;
using Microsoft.AspNetCore.Http;

namespace Bajol.GovFlow.Infrastructure.Persistence.Auditing;

/// <summary>
/// Resolves the audit principal from the current HTTP request claims.
/// </summary>
public sealed class HttpContextAuditUserProvider(IHttpContextAccessor httpContextAccessor) : IAuditUserProvider
{
    /// <inheritdoc />
    public string GetCurrentUserId()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            return SystemAuditUserProvider.SystemPrincipal;
        }

        return user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue(ClaimTypes.Name)
            ?? user.Identity?.Name
            ?? SystemAuditUserProvider.SystemPrincipal;
    }
}
