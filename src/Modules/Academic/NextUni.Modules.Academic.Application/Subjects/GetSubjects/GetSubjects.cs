using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Application.QueryExtension;
using NextUni.Common.Application.User;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;

namespace NextUni.Modules.Academic.Application.Subjects.GetSubjects;

public abstract class GetSubjects
{
    public record Query(int PageNumber, int PageSize, bool IsAdmin) : IQuery<Page<ResponseItem>>, IPageable;
    
    internal sealed class Handler(IAcademicDbContext dbContext) : IQueryHandler<Query, Page<ResponseItem>>
    {
        public async Task<Result<Page<ResponseItem>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = dbContext.Subjects
                .Applypagination(request.PageNumber, request.PageSize)
                .Select(subject => new ResponseItem(subject.Id, subject.Name, subject.IsDeleted));
            
            if (request.IsAdmin)
            {
                query = query.IgnoreQueryFilters();
            }
            
            List<ResponseItem> subjects = await query
                .ToListAsync(cancellationToken);

            int count = await query.CountAsync(cancellationToken);
            
            return new Page<ResponseItem>(
                subjects,
                count,
                request.PageNumber,
                request.PageSize);
        }
    }

    public record ResponseItem(Guid Id, string Name, bool IsDeleted);
}