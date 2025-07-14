using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Events.Application.Abstractions.Data;
using NextUni.Modules.Events.Domain.Events;

namespace NextUni.Modules.Events.Application.EventRegistrations.CancelEventRegistration;

public abstract class CancelEventRegistration
{
    public record Command(Guid RegistrationId) : ICommand;
    
    internal sealed class Handler(IEventDbContext dbContext) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            
            var isRegistrationExist = await dbContext.EventRegistrations.AnyAsync(x => x.Id == request.RegistrationId);
            
            if (!isRegistrationExist)
            {
                return Result.Failure<Guid>(new Error("Registration.NotExisted",
                    $"The registration with Id {request.RegistrationId} does not exist.", ErrorType.NotFound));
            }
            
            var registration = await dbContext.EventRegistrations 
                .SingleAsync(x => x.Id == request.RegistrationId, cancellationToken);

            var eventEntity = await dbContext.Events
                .SingleAsync(x => x.Id == registration.EventId, cancellationToken);

            if (eventEntity.Status != EventStatus.Published)
            {
                return Result.Failure(new Error($"Registration.{eventEntity.Status.ToString()}",
                    $"The Event with Id {eventEntity.Id} is in status {eventEntity.Status}.", ErrorType.NotFound));
            }
            
            dbContext.EventRegistrations.Remove(registration);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success("Registration cancelled successfully.");
        }
    }
}