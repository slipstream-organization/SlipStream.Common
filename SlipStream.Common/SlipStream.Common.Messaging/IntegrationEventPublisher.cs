using MassTransit;
using SlipStream.Common.Events;

namespace SlipStream.Common;

public class IntegrationEventPublisher(IPublishEndpoint publishEndpoint): IIntegrationEventPublisher
{
    public Task Publish<TEvent>(TEvent @event, CancellationToken token) where TEvent : class, IIntegrationEvent
    {
        return publishEndpoint.Publish(@event, token);
    }
}