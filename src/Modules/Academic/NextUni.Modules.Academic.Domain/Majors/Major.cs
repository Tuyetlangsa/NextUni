using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Majors;

public class Major : Entity
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Guid UniversityId { get; set; }
    public bool IsDeleted { get; set; }
}