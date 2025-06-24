using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Application.QueryExtension;
using NextUni.Common.Domain;
using NextUni.Modules.Contents.Application.Abstractions.Data;
using NextUni.Modules.Contents.Domain.CounsellingArticles;

namespace NextUni.Modules.Contents.Application.GetMasterCounsellingArticles;

public abstract class GetMasterCounsellingArticles
{
    public record Query(int PageNumber, int PageSize, bool IsAdmin) : IQuery<Page<Response>>, IPageable;

    internal sealed class Handler(IContentDbContext dbContext) : IQueryHandler<Query, Page<Response>>
    {
        public async Task<Result<Page<Response>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = dbContext.CounsellingArticles
                .OrderByDescending(c => c.PublishAt)
                .AsQueryable();
            
            if (!request.IsAdmin)
            {
                query = query.Where(x => x.Status == CounsellingArticleStatus.Published);
            }
            
            var count = await query.CountAsync(cancellationToken);
            
            var result = await query.Applypagination(
                    request.PageNumber, 
                    request.PageSize)
                .Select(c => new Response(
                    c.Id,
                    c.Title,
                    c.Content,
                    c.Status))
                .ToListAsync(cancellationToken);

            return new Page<Response>(
                result,
                count,
                request.PageNumber,
                request.PageSize);
        }
    }
    public record Response(
        Guid Id,
        string Title,
        string Content,
        CounsellingArticleStatus Status);
}