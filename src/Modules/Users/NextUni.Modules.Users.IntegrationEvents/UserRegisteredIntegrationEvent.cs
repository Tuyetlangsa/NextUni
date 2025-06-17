using NextUni.Common.Application.EventBus;

namespace NextUni.Modules.Users.IntegrationEvents;

public class UserRegistedIntegrationEvent : IntegrationEvent
{
    public UserRegistedIntegrationEvent
        (Guid id, 
        DateTime occurredOnUtc,
        Guid userId,
        string email,
        string firstName,
        string lastName,
        string phoneNumber) 
        : base(id, occurredOnUtc)
    {
        UserId = userId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }
    
    public Guid UserId { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string PhoneNumber { get; }
}