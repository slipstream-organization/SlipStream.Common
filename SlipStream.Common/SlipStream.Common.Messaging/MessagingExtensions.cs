using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SlipStream.Common.Events;

namespace SlipStream.Common;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName,
        Action<MessagingBuilder> configure)
    {
        var builder = new MessagingBuilder(services);
        configure(builder);

        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.Section));
        services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

        services.AddMassTransit(bus =>
        {
            foreach (var register in builder.Consumers)
                register(bus, serviceName);

            bus.UsingRabbitMq((context, cfg) =>
            {
                var o = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
                cfg.Host(o.Host, o.VirtualHost, h =>
                {
                    h.Username(o.Username);
                    h.Password(o.Password);
                });

                cfg.Publish<IIntegrationEvent>(p => p.Exclude = true);
                cfg.UseMessageRetry(r => r.Intervals(500, 2000, 5000));
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}