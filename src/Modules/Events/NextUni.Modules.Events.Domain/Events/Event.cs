using NextUni.Common.Domain;

namespace NextUni.Modules.Events.Domain.Events;

public class Event : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public string Address { get; set; } = null!;
    public bool IsOnline { get; set; }
    public Guid UniversityId { get; set; }
    
    public EventStatus Status { get; set; }

}