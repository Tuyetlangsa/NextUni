using NextUni.Common.Application.EventBus;

namespace NextUni.Modules.Academic.IntegrationEvents;


public class MajorCreatedIntegrationEvent : IntegrationEvent
{
    public MajorCreatedIntegrationEvent
        (Guid id, 
        DateTime occurredOnUtc,
        Guid universityId,
        string textFormatted) 
        : base(id, occurredOnUtc)
    {
        UniversityId = universityId;
        TextFormatted = textFormatted;
    }
    
    public Guid UniversityId { get; } 
    public string TextFormatted { get; }
}