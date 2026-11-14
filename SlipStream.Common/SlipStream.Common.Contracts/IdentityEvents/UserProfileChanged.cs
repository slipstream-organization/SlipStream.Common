using SlipStream.Common.Events;

namespace SlipStream.Common.IdentityEvents;

public sealed class UserProfileChanged : IIntegrationEvent
{
    public Guid EventId { get; set; } = Guid.NewGuid();
    public required Guid UserId { get; set; }
    public required string Handle { get;  set; }
    public required string DisplayName { get; set; }
    public string? FavouriteTeam { get; set; }
    public string? FavouriteDriver { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
}