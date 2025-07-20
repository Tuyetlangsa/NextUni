using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Application.User;
using NextUni.Common.Domain;
using NextUni.Modules.Events.Application.Abstractions.Data;
using NextUni.Modules.Events.Domain.Events;

namespace NextUni.Modules.Events.Application.EventRegistrations.GetEventRegistrations;

public abstract class GetEventRegistrations
{
    public record Query(Guid EventId) : IQuery<Response>;

    public class Response : List<ResponseItem>;

    public record ResponseItem(
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber);
    
    internal sealed class Handler(
        IEventDbContext dbContext,
        ICurrentUser currentUser) : IQueryHandler<Query, Response>
    {
        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            List<EventRegistration> eventRegistrations = await dbContext.EventRegistrations
                .Where(er => er.EventId == query.EventId)
                .ToListAsync(cancellationToken);

            var userIds = eventRegistrations.Select(r => r.UserId).Distinct();

            var users = dbContext.Users.Where(u => userIds.Contains(u.Id)).ToList();

            Response response = new();
            foreach (var eventRegistration in eventRegistrations)
            {
                var user = users.First(u => u.Id == eventRegistration.UserId);
                response.Add(new ResponseItem(
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.PhoneNumber));
            }

            return response;
        }
    }
}