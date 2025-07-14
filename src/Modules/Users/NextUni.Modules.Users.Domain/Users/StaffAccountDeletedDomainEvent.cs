using NextUni.Common.Domain;

namespace NextUni.Modules.Users.Domain.Users;


public class StaffAccountDeletedDomainEvent(Guid userId, Guid universityId) : DomainEvent
{
    public Guid UserId { get; init; } = userId;
    public Guid UniversityId { get; init; } = universityId;
}