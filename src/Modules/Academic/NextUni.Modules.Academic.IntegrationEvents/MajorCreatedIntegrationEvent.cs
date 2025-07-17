using NextUni.Common.Application.EventBus;

namespace NextUni.Modules.Academic.IntegrationEvents;


public class MajorCreatedIntegrationEvent : IntegrationEvent
{
    public MajorCreatedIntegrationEvent
        (Guid id, 
        DateTime occurredOnUtc,
        Guid majorId,
        string textFormatted) 
        : base(id, occurredOnUtc)
    {
        MajorId = majorId;
        TextFormatted = textFormatted;
    }
    
    public Guid MajorId { get; } 
    public string TextFormatted { get; }
}