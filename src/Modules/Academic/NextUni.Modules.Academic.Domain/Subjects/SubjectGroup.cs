using NextUni.Common.Domain;
using NextUni.Modules.Academic.Domain.Majors;

namespace NextUni.Modules.Academic.Domain.Subjects;

public class SubjectGroup : Entity
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    // public string Name { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    public ICollection<MajorSubjectGroupByYear> MajorSubjectGroupByYears { get; set; } = new List<MajorSubjectGroupByYear>();
}