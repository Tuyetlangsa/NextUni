using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Subjects;

public class SubjectGroup : Entity
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}