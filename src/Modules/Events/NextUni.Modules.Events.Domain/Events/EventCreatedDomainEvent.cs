using NextUni.Common.Domain;

namespace NextUni.Modules.Events.Domain.Events;

public class EventCreatedDomainEvent(Guid eventId, string title, string content) : DomainEvent
{
    public Guid EventId { get; } = eventId;
    public string Title { get; } = title;
    public string Content { get; } = content;
}