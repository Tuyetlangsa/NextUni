using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Application.QueryExtension;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.IntroductionBlogs;
using NextUni.Modules.Academic.Domain.Majors;

namespace NextUni.Modules.Academic.Application.Majors.GetMajorById;

public abstract class GetMajorById
{
    public record Query(Guid MajorId, bool IsAdmin) : IQuery<MajorResponse>;

    internal sealed class Handler(IAcademicDbContext dbContext) : IQueryHandler<Query, MajorResponse>
    {
        public async Task<Result<MajorResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = dbContext.Majors.AsQueryable();
            if (request.IsAdmin)
            {
                query = query.IgnoreQueryFilters();
                
            }
            
            var major = await query.FirstOrDefaultAsync(m => m.Id == request.MajorId);
            if (major is null)
            {
                return Result.Failure<MajorResponse>(MajorErrors.NotFound(request.MajorId));
            }

            var subjectGroupQuery = dbContext.SubjectGroups.AsQueryable();
            if (request.IsAdmin)
            {
                subjectGroupQuery = subjectGroupQuery.IgnoreQueryFilters();
            }
            var subjectGroups = await (from link in dbContext.MajorSubjectGroupByYear
                    join sg in subjectGroupQuery
                        on link.SubjectGroupId equals sg.Id
                    where link.MajorId == request.MajorId
                    select new
                    {
                        link.Year,
                        SubjectGroup = new SubjectGroupResponse(sg.Id, sg.Code)
                    })
                .ToListAsync(cancellationToken);
           
            var subjectGroupsByYear = subjectGroups
                .GroupBy(x => x.Year.Year)
                .ToDictionary(
                    g => g.Key, 
                    g => g.Select(x => x.SubjectGroup).ToList());

            var introductionBlog = await dbContext.IntroductionBlogs
                .FirstOrDefaultAsync(b => b.TargetId == request.MajorId && b.IntroductionType == IntroductionType.Major,
                    cancellationToken: cancellationToken);
            
            var response = new MajorResponse(
                major.Id,
                major.Code,
                major.Name,
                introductionBlog?.Title ?? string.Empty,
                introductionBlog?.Content ?? string.Empty,
                major.IsDeleted,
                subjectGroupsByYear);

            return response;
        }
    }

    public record MajorResponse(
        Guid Id,
        string Code,
        string Name,
        string Title,
        string Content,
        bool IsDeleted,
        Dictionary<int, List<SubjectGroupResponse>> SubjectGroupByYear);

    public record SubjectGroupResponse(
        Guid Id,
        string Code);
}