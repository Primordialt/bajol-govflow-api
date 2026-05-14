using Bajol.GovFlow.API.Models;
using Bajol.GovFlow.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Bajol.GovFlow.API.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "Unhandled exception while processing {Method} {Path}", context.Request.Method, context.Request.Path);

        var (statusCode, response) = MapException(context, exception);

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json; charset=utf-8";

        await context.Response.WriteAsJsonAsync(response, cancellationToken: context.RequestAborted);
    }

    private static (int StatusCode, ApiResponse Response) MapException(HttpContext context, Exception exception) =>
        exception switch
        {
            ValidationException validation => (
                StatusCodes.Status400BadRequest,
                ApiResponse.FromFailure(
                    context,
                    "Validation failed.",
                    validation.Errors
                        .Select(e => new ErrorDetail(e.ErrorCode ?? "validation_error", e.ErrorMessage, e.PropertyName))
                        .ToList())),

            AppException app => (
                app.StatusCode,
                ApiResponse.FromFailure(context, app.Message, new[] { new ErrorDetail(app.Code ?? "application_error", app.Message) })),

            DbUpdateConcurrencyException => (
                StatusCodes.Status409Conflict,
                ApiResponse.FromFailure(
                    context,
                    "The record was modified by another process. Refresh and try again.",
                    new[] { new ErrorDetail("concurrency_conflict", "Concurrency conflict.") })),

            _ => (
                StatusCodes.Status500InternalServerError,
                ApiResponse.FromFailure(
                    context,
                    "An unexpected error occurred.",
                    new[] { new ErrorDetail("internal_error", "An unexpected error occurred.") }))
        };
}
