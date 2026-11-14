using System.Text.RegularExpressions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using SlipStream.Common.Events;

namespace SlipStream.Common;

public sealed partial class MessagingBuilder(IServiceCollection services)
{
    private readonly HashSet<Type> _events = [];
    internal List<Action<IBusRegistrationConfigurator, string>> Consumers { get; } = [];

    public MessagingBuilder Handle<TEvent, THandler>()
        where TEvent : class, IIntegrationEvent
        where THandler : class, IIntegrationEventHandler<TEvent>
    {
        if (!_events.Add(typeof(TEvent)))
            throw new InvalidOperationException($"Подія {typeof(TEvent).Name} вже має хендлер.");

        services.AddScoped<IIntegrationEventHandler<TEvent>, THandler>();

        Consumers.Add((bus, serviceName) => bus
            .AddConsumer<IntegrationEventConsumer<TEvent>>()
            .Endpoint(e => e.Name = $"{serviceName}-{ToKebabCase(typeof(TEvent).Name)}"));

        return this;
    }

    private static string ToKebabCase(string name) => KebabRegex().Replace(name, "$1-$2").ToLowerInvariant();

    [GeneratedRegex("([a-z0-9])([A-Z])")]
    private static partial Regex KebabRegex();
}