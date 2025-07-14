using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Events.Application.Abstractions.Data;
using NextUni.Modules.Events.Domain.Events;

namespace NextUni.Modules.Events.Application.Events.GetEventById;

public class GetEventById
{
    public record Query(Guid Id) : IQuery<Response>;
    
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
    
    internal sealed class Handler(IEventDbContext dbContext) : IQueryHandler<Query, Response>
    {
        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var eventEntity = await dbContext.Events.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (eventEntity is null)
            {
                return Result.Failure<Response>(EventErrors.NotFound(request.Id));
            }
            
            var introductionBlog = await dbContext.IntroductionBlogs
                .SingleAsync(b => b.TargetId == eventEntity.Id, cancellationToken);
            
            
            return new Response(
                eventEntity.Id,
                eventEntity.Name,
                eventEntity.StartDate,
                eventEntity.Address,
                eventEntity.IsOnline,
                eventEntity.Status)
            {
                Title = introductionBlog.Title,
                Content = introductionBlog.Content
            };
        }
    }
}