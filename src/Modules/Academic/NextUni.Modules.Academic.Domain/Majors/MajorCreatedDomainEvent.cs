using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Majors;

public class MajorCreatedDomainEvent(Guid universityId) : DomainEvent
{
    public Guid UniversityId { get;} = universityId;
}