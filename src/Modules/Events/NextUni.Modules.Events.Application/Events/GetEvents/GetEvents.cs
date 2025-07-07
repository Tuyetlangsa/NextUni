using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Application.QueryExtension;
using NextUni.Common.Domain;
using NextUni.Modules.Events.Application.Abstractions.Data;
using NextUni.Modules.Events.Domain.Events;
using NextUni.Modules.Events.Domain.IntroductionBlogs;

namespace NextUni.Modules.Events.Application.Events.GetEvents;

public abstract class GetEvents
{
    public record Query(
        int PageNumber, 
        int PageSize, 
        QueryStatus Statustatus,
        bool IsAdminOrStaff) 
        : IQuery<Page<Response>>, IPageable;

    public record Response(
        Guid Id,
        string Name,
        DateOnly StartDate,
        string Address,
        bool IsOnline,
        EventStatus Status)
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
    };
    public enum QueryStatus 
    {
        Pending, 
        Published, 
        Ongoing, 
        Completed, 
        Canceled, 
        Rejected
    }
    
    internal sealed class Handler(IEventDbContext dbContext) : IQueryHandler<Query, Page<Response>>
    {
        public async Task<Result<Page<Response>>> Handle(Query request, CancellationToken cancellationToken)
        {

              var query = dbContext.Events
                .OrderByDescending(e => e.StartDate)
                .AsQueryable();

              if (request.IsAdminOrStaff)
              {
                  query = query.IgnoreQueryFilters();
                  query = request.Statustatus switch
                  {
                      QueryStatus.Pending => query.Where(x => x.Status == Domain.Events.EventStatus.Pending),
                      QueryStatus.Published => query.Where(x => x.Status == Domain.Events.EventStatus.Published),
                      QueryStatus.Ongoing => query.Where(x => x.Status == Domain.Events.EventStatus.Ongoing),
                      QueryStatus.Completed => query.Where(x => x.Status == Domain.Events.EventStatus.Completed),
                      QueryStatus.Canceled => query.Where(x => x.Status == Domain.Events.EventStatus.Cancelled),
                      QueryStatus.Rejected => query.Where(x => x.Status == Domain.Events.EventStatus.Rejected),
                  };
              }
              else
              {
                  query = request.Statustatus switch
                  {
                      QueryStatus.Published => query.Where(x => x.Status == Domain.Events.EventStatus.Published),
                      QueryStatus.Ongoing => query.Where(x => x.Status == Domain.Events.EventStatus.Ongoing),
                      QueryStatus.Completed => query.Where(x => x.Status == Domain.Events.EventStatus.Completed),
                      QueryStatus.Canceled => query.Where(x => x.Status == Domain.Events.EventStatus.Cancelled),
                  };
              }
              
              
            int count = await query.CountAsync(cancellationToken);
            
            var result = await query.Applypagination(request.PageNumber, request.PageSize)
                .Select(e => new Response(
                    e.Id,
                    e.Name,
                    e.StartDate,
                    e.Address,
                    e.IsOnline,
                    e.Status
                    ))
                .ToListAsync(cancellationToken);
              
            var eventIds = result.Select(e => e.Id).ToList();
            var introductionBlogs = await dbContext.IntroductionBlogs
                .Where(b => eventIds.Contains(b.TargetId) && b.IntroductionType == IntroductionType.Event)
                .ToDictionaryAsync(b => b.TargetId, b => b, cancellationToken: cancellationToken);

            foreach (var response in result)
            {
                if (introductionBlogs.TryGetValue(response.Id, out var intro))
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
            
            return new Page<Response>(result, count, request.PageNumber, request.PageSize);
        }
    }
}