using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Majors;

public class MajorCreatedDomainEvent(Guid majorId, string title, string content) : DomainEvent
{
    public Guid MajorId { get;} = majorId;
    public string Title { get;} = title;
    public string Content { get;} = content;
}