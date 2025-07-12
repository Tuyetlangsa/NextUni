using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;

namespace NextUni.Modules.Academic.Application.Majors.GetAdmissionScoresByYear;

public abstract class GetAdmissionScoreByYear
{
    public record Query(DateOnly Year, Guid UniversityId) : IQuery<Dictionary<Guid, AdmissionScore>>;

    internal sealed class Handler(IAcademicDbContext dbContext) : IQueryHandler<Query, Dictionary<Guid, AdmissionScore>>
    {
        public async Task<Result<Dictionary<Guid, AdmissionScore>>> Handle(Query request, CancellationToken cancellationToken)
        {
            // var result = await dbContext.AdmissionScores
            //     .Where(s => s.Year.Year == request.Year.Year)
            //     .Include(a => a.Major)
            //     .ToDictionaryAsync(
            //         g => g.MajorId, 
            //         g => new AdmissionScore( g.Major.Name, g.GpaScore, g.ExamScore), cancellationToken: cancellationToken);
            // return result;

            var result = await dbContext.Majors
                .Where(m => m.UniversityId == request.UniversityId)
                .GroupJoin(
                    dbContext.AdmissionScores.Where(s => s.Year.Year == request.Year.Year),
                    major => major.Id,
                    score => score.MajorId,
                    (major, scores) => new
                    {
                        MajorId = major.Id,
                        MajorName = major.Name,
                        Score = scores.FirstOrDefault()
                    }
                )
                .ToDictionaryAsync(
                    x => x.MajorId,
                    x => new AdmissionScore(
                        x.MajorName,
                        x.Score?.GpaScore ?? 0,  
                        x.Score?.ExamScore ?? 0
                    ),
                    cancellationToken
                );
            
            return result;
        }
    }

    public record AdmissionScore(string MajorName, float GpaScore, float ExamScore);
}