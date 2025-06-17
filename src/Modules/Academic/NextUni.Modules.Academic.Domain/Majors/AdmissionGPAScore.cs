using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Majors;

public class AdmissionGPAScore : Entity
{
    public Guid Id { get; set; }
    public Guid MajorId { get; set; }
    public DateOnly Year { get; set; }
    public float Score { get; set; }
}