using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Application.QueryExtension;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.IntroductionBlogs;

namespace NextUni.Modules.Academic.Application.Majors.GetMajors;

public abstract class GetMajors
{
    public record Query(int PageNumber, int PageSize, bool IsAdmin) : IQuery<Page<MajorResponse>>, IPageable;

    internal sealed class Handler(IAcademicDbContext dbContext) : IQueryHandler<Query, Page<MajorResponse>>
    {
        public async Task<Result<Page<MajorResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = dbContext.Majors.AsQueryable();
            if (request.IsAdmin)
            {
                query = query.IgnoreQueryFilters();
                
            }

            int count = await query.CountAsync();
            var paginated = await query
                .Applypagination(request.PageNumber, request.PageSize)
                .ToListAsync(cancellationToken);
            
            var majorIds = paginated.Select(x => x.Id).ToList();

            var subjectGroupQuery = dbContext.SubjectGroups.AsQueryable();
            if (request.IsAdmin)
            {
                subjectGroupQuery = subjectGroupQuery.IgnoreQueryFilters();
            }
            
            var subjectGroups = await (from link in dbContext.MajorSubjectGroupByYear
                    join sg in subjectGroupQuery
                        on link.SubjectGroupId equals sg.Id
                    where majorIds.Contains(link.MajorId)
                    select new
                    {
                        link.MajorId, 
                        link.Year, 
                        SubjectGroup = new SubjectGroupResponse(sg.Id, sg.Code)
                    })
                .ToListAsync(cancellationToken);
            
            var subjectGroupsByMajorId = subjectGroups
                .GroupBy(x => x.MajorId)
                .ToDictionary(
                    g => g.Key, 
                    g => g
                        .GroupBy(x => x.Year)
                        .ToDictionary(
                        g1 => g1.Key, 
                        g1 => g1.Select(x => x.SubjectGroup).ToList()));

            var majorIdToIntroductionBlog = await dbContext.IntroductionBlogs
                .Where(b => majorIds.Contains(b.TargetId)
                            && b.IntroductionType == IntroductionType.Major)
                .ToDictionaryAsync(b => b.TargetId, b => b, cancellationToken: cancellationToken);
            
            
            var result = paginated.Select(m =>
            {
                subjectGroupsByMajorId.TryGetValue(m.Id, out var yearGroups);
                majorIdToIntroductionBlog.TryGetValue(m.Id, out var introduction);
                return new MajorResponse(
                    m.Id,
                    m.Code,
                    m.Name,
                    introduction!.Title,
                    introduction.Content,
                    m.IsDeleted,
                    yearGroups!);
            })
            .ToList();

            return new Page<MajorResponse>(result, count, request.PageNumber, request.PageSize);
        }
    }

    public record MajorResponse(
        Guid Id,
        string Code,
        string Name,
        string Title,
        string Content,
        bool IsDeleted,
        Dictionary<DateOnly, List<SubjectGroupResponse>> SubjectGroupByYear);

    public record SubjectGroupResponse(
        Guid Id,
        string Code);
}