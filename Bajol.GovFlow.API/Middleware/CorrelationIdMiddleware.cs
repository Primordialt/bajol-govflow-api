using System.Diagnostics;

namespace Bajol.GovFlow.API.Middleware;

public sealed class CorrelationIdMiddleware(RequestDelegate next)
{
    public const string HeaderName = "X-Correlation-Id";

    public Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[HeaderName].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString("D");
        }

        context.Response.Headers[HeaderName] = correlationId;
        context.Items[HeaderName] = correlationId;

        if (Activity.Current is not null)
        {
            Activity.Current.AddTag("correlation.id", correlationId);
        }

        return next(context);
    }
}
