using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SlipStream.Common.Errors;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is OperationCanceledException && httpContext.RequestAborted.IsCancellationRequested)
        {
            httpContext.Response.StatusCode = 499;
            return true;
        }

        var (status, title) = exception switch
        {
            ValidationException   => (StatusCodes.Status400BadRequest, "Validation failed"),
            UnauthorizedException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
            ForbiddenException    => (StatusCodes.Status403Forbidden, "Forbidden"),
            NotFoundException     => (StatusCodes.Status404NotFound, "Not found"),
            ConflictException     => (StatusCodes.Status409Conflict, "Conflict"),
            _                     => (StatusCodes.Status500InternalServerError, "Internal server error")
        };

        if (status >= 500)
            logger.LogError(exception, "Unhandled exception");
        else
            logger.LogWarning("{ExceptionType}: {Message}", exception.GetType().Name, exception.Message);

        var problem = new ProblemDetails { Status = status, Title = title };

        switch (exception)
        {
            case ValidationException ve:
                problem.Extensions["errors"] = ve.Errors;
                break;
            case AppException:
                problem.Detail = exception.Message;
                break;
        }

        httpContext.Response.StatusCode = status;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problem,
            Exception = exception
        });
    }
}