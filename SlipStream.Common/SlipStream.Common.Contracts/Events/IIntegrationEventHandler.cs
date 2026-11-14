namespace SlipStream.Common.Events;

public interface IIntegrationEventHandler<in TEvent> where TEvent: IIntegrationEvent
{
    public Task Handle(TEvent @event, CancellationToken ct);
}