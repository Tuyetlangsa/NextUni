using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Majors;

public class MajorSubjectGroupByYear : Entity
{
    public Guid Id { get; set; }
    public Guid MajorId { get; set; }
    public Guid SubjectGroupId { get; set; }
    public DateOnly Year { get; set; }
}