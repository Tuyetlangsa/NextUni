using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Universities;

public class UniversityCreatedDomainEvent(Guid universityId) : DomainEvent
{
    public Guid UniversityId { get;} = universityId;

};