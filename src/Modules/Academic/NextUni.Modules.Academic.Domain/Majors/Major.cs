using NextUni.Common.Domain;
using NextUni.Modules.Academic.Domain.Subjects;
using NextUni.Modules.Academic.Domain.Universities;

namespace NextUni.Modules.Academic.Domain.Majors;

public class Major : Entity
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Guid UniversityId { get; set; }
    public bool IsDeleted { get; set; }
    
    //navigation property
    public virtual University University { get; set; } = null!;
    public virtual ICollection<SubjectGroup> SubjectGroups { get; set; } = new List<SubjectGroup>();
    
    public virtual ICollection<AdmissionGPAScore> AdmissionGPAScores { get; set; } = new List<AdmissionGPAScore>();
    public virtual ICollection<AdmissionExamScore> AdmissionExamScores { get; set; } = new List<AdmissionExamScore>();
}