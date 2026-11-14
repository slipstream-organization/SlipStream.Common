using MassTransit;
using SlipStream.Common.Events;

namespace SlipStream.Common;

public sealed class IntegrationEventConsumer<TEvent>(IIntegrationEventHandler<TEvent> handler) : IConsumer<TEvent> where TEvent: class, IIntegrationEvent
{
    public Task Consume(ConsumeContext<TEvent> context)
    {
        return handler.Handle(context.Message, context.CancellationToken);
    }
}