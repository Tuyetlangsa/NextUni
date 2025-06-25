using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Events.Application.Abstractions.Data;
using NextUni.Modules.Events.Domain.Events;

namespace NextUni.Modules.Events.Application.EventRegistrations;

public abstract class CreateEventRegistration
{
    public record Command(Guid EventId, Guid UserId) : ICommand<Guid>;
    
    internal sealed class Handler(IEventDbContext dbContext) : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var isUserExist = await dbContext.Users.AnyAsync(x => x.Id == request.UserId);
            if (!isUserExist)
            {
                return Result.Failure<Guid>(new Error("User.NotExisted",
                    $"The User with Id {request.UserId} does not exist.", ErrorType.NotFound));
            }
            var isEventExist = await dbContext.Events.AnyAsync(x => x.Id == request.EventId);
            
            if (!isEventExist)
            {
                return Result.Failure<Guid>(new Error("Event.NotExisted",
                    $"The Event with Id {request.EventId} does not exist.", ErrorType.NotFound));
            }
            
            var registration = new EventRegistration()
            {
                Id = Guid.NewGuid(),
                EventId = request.EventId,
                UserId = request.UserId,
                Status = true,
            };
            
            dbContext.EventRegistrations.Add(registration);
            await dbContext.SaveChangesAsync(cancellationToken);
            return registration.Id;
        }
    }
}