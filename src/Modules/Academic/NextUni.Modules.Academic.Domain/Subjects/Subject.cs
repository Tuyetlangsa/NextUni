using NextUni.Common.Domain;
using NextUni.Modules.Academic.Domain.IntroductionBlogs;

namespace NextUni.Modules.Academic.Domain.Subjects;

public class Subject : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    
    public bool IsDeleted { get; set; } 
    
    public virtual ICollection<SubjectGroup> SubjectGroups { get; set; } = new List<SubjectGroup>();
    
}