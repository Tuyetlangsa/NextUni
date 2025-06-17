using NextUni.Common.Domain;

namespace NextUni.Modules.Events.Domain.Events;

public class EventRegistration : Entity
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public bool Status { get; set; } // registed, cancelled
}