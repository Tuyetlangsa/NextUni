using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Universities;

public class University : Entity
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Region Region { get; set; }
    public UniversityType UniversityType { get; set; }
    public string Address { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string WebsiteUrl { get; set; } = null!;
    public string FacebookUrl { get; set; } = null!;
    public bool IsDeleted { get; set; }
}