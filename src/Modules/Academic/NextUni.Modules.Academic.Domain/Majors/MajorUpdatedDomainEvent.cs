using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Majors
{
    public class MajorUpdatedDomainEvent (Guid majorId, Guid id, string title, string content) : DomainEvent
    {
        public Guid MajorId { get; } = majorId;
        public Guid Id { get; } = id;
        public string Title { get; } = title;
        public string Content { get; } = content;
    }
}
