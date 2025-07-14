using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Application.User;
using NextUni.Common.Domain;
using NextUni.Modules.Events.Application.Abstractions.Data;
using NextUni.Modules.Events.Domain.Events;

namespace NextUni.Modules.Events.Application.EventRegistrations.CreateEventRegistration;

public abstract class CreateEventRegistration
{
    public record Command(Guid EventId) : ICommand<Guid>;
    
    internal sealed class Handler(IEventDbContext dbContext, ICurrentUser currentUser) : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = currentUser.UserId;
            var isUserExist = await dbContext.Users.AnyAsync(x => x.Id == userId);
            if (!isUserExist)
            {
                return Result.Failure<Guid>(new Error("User.NotExisted",
                    $"The User with Id {userId} does not exist.", ErrorType.NotFound));
            }
            var isEventExist = await dbContext.Events.AnyAsync(x => x.Id == request.EventId);
            
            if (!isEventExist)
            {
                return Result.Failure<Guid>(new Error("Event.NotExisted",
                    $"The Event with Id {request.EventId} does not exist.", ErrorType.NotFound));
            }
            var eventEntity = await dbContext.Events.FirstOrDefaultAsync(x => x.Id == request.EventId);
            if (eventEntity is null)
            {
                return Result.Failure<Guid>(EventErrors.NotFound(request.EventId));
            }

            if (eventEntity.Status != EventStatus.Published)
            {
                return Result.Failure<Guid>(new Error("Event.NotPublished",
                    $"The Event with Id {request.EventId} is not published.", ErrorType.Validation));
            }
            var registration = new EventRegistration()
            {
                Id = Guid.NewGuid(),
                EventId = request.EventId,
                UserId = userId,
            };
            
            dbContext.EventRegistrations.Add(registration);
            await dbContext.SaveChangesAsync(cancellationToken);
            return registration.Id;
        }
    }
}