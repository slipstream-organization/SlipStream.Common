using SlipStream.Common.Events;

namespace SlipStream.Common.Events;

public interface IIntegrationEventPublisher
{
    Task Publish<TEvent>(TEvent @event, CancellationToken token) where TEvent: class, IIntegrationEvent;
}