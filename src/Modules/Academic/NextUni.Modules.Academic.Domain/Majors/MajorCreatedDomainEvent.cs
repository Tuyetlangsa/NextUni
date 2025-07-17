using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Majors;

public class MajorCreatedDomainEvent(Guid majorId) : DomainEvent
{
    public Guid MajorId { get;} = majorId;
}