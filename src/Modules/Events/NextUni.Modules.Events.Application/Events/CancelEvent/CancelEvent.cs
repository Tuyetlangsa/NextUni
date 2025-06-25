using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Clock;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Events.Application.Abstractions.Data;
using NextUni.Modules.Events.Domain.Events;

namespace NextUni.Modules.Events.Application.Events.CancelEvent
{
    public abstract class CancelEvent
    {
        public record Command(Guid Id) : ICommand<Guid>;

        internal sealed class Handler(
        IEventDbContext dbContext,
        IDateTimeProvider dateTimeProvider) : ICommandHandler<Command, Guid>
        {
            public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
            {
                var requestEvent = await dbContext.Events.FirstOrDefaultAsync(e => e.Id == command.Id, cancellationToken);
                if (requestEvent is null)
                {
                    return Result.Failure<Guid>(EventErrors.NotFound(command.Id));
                }

                if (requestEvent.Status == EventStatus.Ongoing || requestEvent.Status == EventStatus.Completed || requestEvent.Status == EventStatus.Cancelled)
                {
                    return Result.Failure<Guid>(EventErrors.IncorrectStatus(command.Id, requestEvent.Status));
                }

                requestEvent.Status = EventStatus.Cancelled;
                dbContext.Events.Update(requestEvent);
                await dbContext.SaveChangesAsync(cancellationToken);
                return requestEvent.Id;
            }
        }
    }
}
