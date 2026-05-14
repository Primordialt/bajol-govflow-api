using System.Diagnostics;

namespace Bajol.GovFlow.API.Models;

public sealed class ApiResponse
{
    public bool Success { get; init; }
    public object? Data { get; init; }
    public string? Message { get; init; }
    public IReadOnlyList<ErrorDetail>? Errors { get; init; }
    public string TraceId { get; init; } = string.Empty;

    public static ApiResponse FromSuccess(object? data, HttpContext httpContext, string? message = null) =>
        new()
        {
            Success = true,
            Data = data,
            Message = message,
            TraceId = ResolveTraceId(httpContext)
        };

    public static ApiResponse FromFailure(
        HttpContext httpContext,
        string message,
        IReadOnlyList<ErrorDetail>? errors = null) =>
        new()
        {
            Success = false,
            Message = message,
            Errors = errors,
            TraceId = ResolveTraceId(httpContext)
        };

    private static string ResolveTraceId(HttpContext httpContext) =>
        Activity.Current?.TraceId.ToString() ?? httpContext.TraceIdentifier;
}

public sealed record ErrorDetail(string Code, string Message, string? Field = null);
