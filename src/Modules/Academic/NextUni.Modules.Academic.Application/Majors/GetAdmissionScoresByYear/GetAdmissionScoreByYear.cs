using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;

namespace NextUni.Modules.Academic.Application.Majors.GetAdmissionScoresByYear;

public abstract class GetAdmissionScoreByYear
{
    public record Query(int Year, Guid UniversityId) : IQuery<List<AdmissionScore>>;

    internal sealed class Handler(IAcademicDbContext dbContext) : IQueryHandler<Query, List<AdmissionScore>>
    {
        public async Task<Result<List<AdmissionScore>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entities  = await dbContext.Majors
                .Where(m => m.UniversityId == request.UniversityId)
                .GroupJoin(
                    dbContext.AdmissionScores.Where(s => s.Year.Year == request.Year),
                    major => major.Id,
                    score => score.MajorId,
                    (major, scores) => new
                    {
                        MajorId = major.Id,
                        MajorName = major.Name,
                        Score = scores.FirstOrDefault()
                    }
                )
                .ToListAsync(cancellationToken);


            var result = entities.Select(x => new AdmissionScore(
                x.MajorId,
                x.MajorName,
                x.Score?.GpaScore ?? 0,
                x.Score?.ExamScore ?? 0
            )).ToList();
            
            
            return result;
        }
    }

    public record AdmissionScore(Guid MajorId, string MajorName, float GpaScore, float ExamScore);
}