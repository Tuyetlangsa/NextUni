using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Subjects;

public class Subject : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}