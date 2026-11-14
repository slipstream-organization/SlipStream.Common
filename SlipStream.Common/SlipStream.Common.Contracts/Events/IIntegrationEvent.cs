namespace SlipStream.Common.Events;

public interface IIntegrationEvent
{
    public Guid EventId { get; set; }
    public DateTime OccurredAt { get; set; }
}