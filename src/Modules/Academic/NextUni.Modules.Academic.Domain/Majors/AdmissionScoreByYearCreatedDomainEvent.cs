using NextUni.Common.Domain;

namespace NextUni.Modules.Academic.Domain.Majors;

public class AdmissionScoreByYearCreatedDomainEvent(Guid universityId, int year) : DomainEvent
{
    public Guid UniversityId { get;} = universityId;
    public int Year { get; } = year;
}