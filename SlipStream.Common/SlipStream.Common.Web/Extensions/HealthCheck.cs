using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace SlipStream.Common.Extensions;

public static class HealthCheck
{
    public static IServiceCollection AddHealthCheck(this IServiceCollection services, string postgreConnection, string redisConnection)
    {
        services.AddHealthChecks()
            .AddNpgSql(postgreConnection, name: "postgreSQL", tags: ["ready"])
            .AddRedis(redisConnection, name: "redis", tags: ["ready"]);

        return services;
    }

    public static IEndpointRouteBuilder UseHealthCheck(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("health/live", new HealthCheckOptions()
        {
            Predicate = _ => false
        });
        
        endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).AllowAnonymous();
        
        return endpoints;
    }
}