using NextUni.Common.Domain;
using NextUni.Modules.Academic.Domain.IntroductionBlogs;
using NextUni.Modules.Academic.Domain.Majors;

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
    
    //navigation property
    public virtual List<Major> Majors { get; set; } = new  List<Major>();
    
}