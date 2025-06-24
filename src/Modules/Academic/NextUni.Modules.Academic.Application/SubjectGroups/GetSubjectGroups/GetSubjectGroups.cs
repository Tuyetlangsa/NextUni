using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Application.QueryExtension;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;

namespace NextUni.Modules.Academic.Application.SubjectGroups.GetSubjectGroups;

public abstract class GetSubjectGroups
{
    public record Query(int PageNumber, int PageSize, bool IsAdmin) : IQuery<Page<Response>>, IPageable;

    internal sealed class Handler(IAcademicDbContext dbContext) : IQueryHandler<Query, Page<Response>>
    {
        public async Task<Result<Page<Response>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = dbContext.SubjectGroups.AsNoTracking().AsQueryable();

            if (request.IsAdmin)
            {
                query = query.IgnoreQueryFilters();
            }
            
            var count = await query.CountAsync(cancellationToken);

            var responses = await query.Applypagination(request.PageNumber, request.PageSize)
                .Select(subjectGroup => new Response(
                    subjectGroup.Id,
                    subjectGroup.Code,
                    subjectGroup.IsDeleted,
                    subjectGroup.Subjects.Select(s => new Subject(s.Id, s.Name)).ToList()))
                .ToListAsync(cancellationToken);

            return Result.Success(new Page<Response>(responses, count, request.PageNumber, request.PageSize));
        }
    }
    public record Response(
        Guid Id,
        string Code,
        bool IsDeleted,
        List<Subject> Subjects);
    
    public record Subject(
        Guid Id,
        
        string Name);
}