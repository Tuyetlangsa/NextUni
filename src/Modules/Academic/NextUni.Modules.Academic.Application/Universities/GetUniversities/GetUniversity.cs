using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Application.QueryExtension;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Domain.IntroductionBlogs;
using NextUni.Modules.Academic.Domain.Universities;

namespace NextUni.Modules.Academic.Application.Universities.GetUniversities;

public abstract class GetUniversity
{
    public record Query(int PageNumber, int PageSize, QueryFilter QueryFilter, bool IsAdmin) : IPageable, IQuery<Page<ResponseItem>>;

    public enum QueryFilter
    {
        North,
        Central,
        South,
        None
    }
    
    internal sealed class Handler(IAcademicDbContext dbContext) : IQueryHandler<Query, Page<ResponseItem>>
    {
        public async Task<Result<Page<ResponseItem>>> Handle(Query request, CancellationToken cancellationToken)
        {
            // get universities filter by region
            

            var query = dbContext.Universities.AsNoTracking().AsQueryable();

            if (request.IsAdmin)
            {
                query = query.IgnoreQueryFilters();
            }
            query = request.QueryFilter switch
            {
                QueryFilter.North => query.Where(u => u.Region == Region.North),
                QueryFilter.South => query.Where(u => u.Region == Region.South),
                QueryFilter.Central => query.Where(u => u.Region == Region.Central),
                QueryFilter.None => query
            };


            var count = await query.CountAsync(cancellationToken);

            var responses = await query.Applypagination(request.PageNumber, request.PageSize).Select(university => new ResponseItem(
                university.Id, 
                university.Code,
                university.Name, 
                university.Region, 
                university.Email, 
                university.WebsiteUrl, 
                university.FacebookUrl,
                university.IsDeleted)).ToListAsync(cancellationToken);
            
            var universityIds = responses.Select(u => u.Id).ToList();
            var universityIdToIntroductionBlog = await dbContext.IntroductionBlogs
                .Where(b => universityIds.Contains(b.TargetId)
                            && b.IntroductionType == IntroductionType.University)
                .ToDictionaryAsync(b => b.TargetId, b => b, cancellationToken: cancellationToken);

            foreach (var response in responses)
            {
                if (universityIdToIntroductionBlog.TryGetValue(response.Id, out var intro))
                {
                    response.Title = intro.Title;
                    response.Content = intro.Content;
                }
                else
                {
                    response.Title = string.Empty;
                    response.Content = string.Empty;
                }
            }
            
            return new Page<ResponseItem>(responses, count, request.PageNumber, request.PageSize);
        }
    }

    public record ResponseItem(
        Guid Id,
        string Code,
        string Name,
        Region Region,
        string Email,
        string WebsiteUrl,
        string FacebookUrl,
        bool IsDeleted
    )
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
    };
}
