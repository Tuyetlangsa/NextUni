using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Majors;

public class AdmissionScore : Entity
{
    public Guid Id { get; set; }
    public Guid MajorId { get; set; }
    public DateOnly Year { get; set; }
    public float GpaScore { get; set; }
    public float ExamScore { get; set; }
    public virtual Major Major { get; set; } = null!;
}