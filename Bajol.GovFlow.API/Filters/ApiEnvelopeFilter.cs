using Bajol.GovFlow.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bajol.GovFlow.API.Filters;

public sealed class ApiEnvelopeFilter : IAsyncResultFilter
{
    public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult)
        {
            var statusCode = objectResult.StatusCode ?? context.HttpContext.Response.StatusCode;
            if (statusCode is >= 200 and < 300 && objectResult.Value is not ApiResponse)
            {
                if (objectResult.Value is ProblemDetails)
                {
                    return next();
                }

                objectResult.Value = ApiResponse.FromSuccess(objectResult.Value, context.HttpContext);
            }
        }

        return next();
    }
}
