using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SlipStream.Common.Behaviors;

namespace SlipStream.Common.Extensions;

public static class MediatorServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (assemblies.Length is 0)
        {
            throw new ArgumentException("Pass at least one assembly with handlers and validators.", nameof(assemblies));
        }

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assemblies);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssemblies(assemblies);

        return services;
    }
}