using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bajol.GovFlow.Application.Common.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var traceId = Activity.Current?.Id ?? Activity.Current?.TraceId.ToString() ?? string.Empty;

        logger.LogInformation("Handling {Request} TraceId={TraceId}", requestName, traceId);

        var sw = Stopwatch.StartNew();
        try
        {
            var response = await next();
            sw.Stop();
            logger.LogInformation("Handled {Request} in {ElapsedMs}ms TraceId={TraceId}", requestName, sw.ElapsedMilliseconds, traceId);
            return response;
        }
        catch (Exception ex)
        {
            sw.Stop();
            logger.LogError(ex, "Error handling {Request} after {ElapsedMs}ms TraceId={TraceId}", requestName, sw.ElapsedMilliseconds, traceId);
            throw;
        }
    }
}
