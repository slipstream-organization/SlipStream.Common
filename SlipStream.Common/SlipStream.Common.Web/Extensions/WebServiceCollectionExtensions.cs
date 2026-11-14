using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SlipStream.Common.Errors;

namespace SlipStream.Common.Extensions;

public static class WebServiceCollectionExtensions
{
    public static IServiceCollection AddPlatformWeb(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = ctx =>
            {
                ctx.ProblemDetails.Extensions["traceId"] = ctx.HttpContext.TraceIdentifier;
            };
        });

        services.AddExceptionHandler<GlobalExceptionHandler>();
        return services;
    }

    public static IApplicationBuilder UsePlatformWeb(this IApplicationBuilder app)
    {
        app.UseExceptionHandler();
        return app;
    }
}