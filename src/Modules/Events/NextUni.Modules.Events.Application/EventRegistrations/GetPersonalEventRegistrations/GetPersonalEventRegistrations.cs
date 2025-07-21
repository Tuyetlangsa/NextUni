using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Application.User;
using NextUni.Common.Domain;
using NextUni.Modules.Events.Application.Abstractions.Data;
using NextUni.Modules.Events.Domain.Events;

namespace NextUni.Modules.Events.Application.EventRegistrations.GetPersonalEventRegistrations;


public abstract class GetPersonalEventRegistrations
{
    public record Query() : IQuery<Response>;

    public class Response : List<ResponseItem>;

    public record ResponseItem(
        Guid EventId,
        string EventName,
        DateOnly EventDate,
        EventStatus EventStatus
    );

    internal sealed class Handler(
        IEventDbContext dbContext,
        ICurrentUser currentUser) : IQueryHandler<Query, Response>
    {
        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var userId = currentUser.UserId;

            var registrations = await dbContext.EventRegistrations
                .Where(er => er.UserId == userId)
                .Include(er => er.Event)                
                .ToListAsync(cancellationToken);

            Response response = new();

            foreach (var reg in registrations)
            {
                response.Add(new ResponseItem(
                    reg.EventId,
                    reg.Event.Name,
                    reg.Event.StartDate,
                    reg.Event.Status
                ));
            }

            return response;
        }
    }
}