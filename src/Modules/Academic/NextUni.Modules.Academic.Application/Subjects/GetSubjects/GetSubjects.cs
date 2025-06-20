using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Application.QueryExtension;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.Subjects;

namespace NextUni.Modules.Academic.Application.Subjects.GetSubjects;

public abstract class CreateSubject
{
    public record Query(int PageNumber, int PageSize) : IQuery<Page<ResponseItem>>, IPageable;
    
    internal sealed class Handler(IAcademicDbContext dbContext) : IQueryHandler<Query, Page<ResponseItem>>
    {
        public async Task<Result<Page<ResponseItem>>> Handle(Query query, CancellationToken cancellationToken)
        {
            List<ResponseItem> subjects = await dbContext.Subjects
                .Applypagination(query.PageNumber, query.PageSize)
                .Select(subject => new ResponseItem(subject.Id, subject.Name))
                .ToListAsync(cancellationToken);
            
            int count = await dbContext.Subjects.CountAsync(cancellationToken: cancellationToken);
            
            return new Page<ResponseItem>(
                subjects,
                count,
                query.PageNumber,
                query.PageSize);
        }
    }

    public record ResponseItem(Guid Id, string Name);
}