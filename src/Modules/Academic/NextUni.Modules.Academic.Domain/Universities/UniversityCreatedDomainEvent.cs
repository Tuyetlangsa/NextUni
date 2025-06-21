using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Universities;

public class UniversityCreatedDomainEvent(Guid universityId, string title, string content) : DomainEvent
{
    public Guid UniversityId { get;} = universityId;
    public string Title { get;} = title;
    public string Content { get;} = content;
};