using NextUni.Common.Application.EventBus;

namespace NextUni.Modules.Users.IntegrationEvents;


public class StaffAccountCreatedIntegrationEvent : IntegrationEvent
{
    public StaffAccountCreatedIntegrationEvent
    (Guid id, 
        DateTime occurredOnUtc,
        Guid userId,
        Guid universityId) 
        : base(id, occurredOnUtc)
    {
        UserId = userId;
        UniversityId = universityId;
    }
    
    public Guid UserId { get; }
    public Guid UniversityId { get; init; }
}