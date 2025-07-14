using MediatR;
using NextUni.Common.Application.EventBus;
using NextUni.Common.Application.Exceptions;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Users.Application.Users.GetUser;
using NextUni.Modules.Users.Domain.Users;
using NextUni.Modules.Users.IntegrationEvents;

namespace NextUni.Modules.Users.Application.Staffs.DeleteStaffAccount;


public class StaffAccountDeletedDomainEventHandler(ISender sender, IEventBus bus) : DomainEventHandler<StaffAccountDeletedDomainEvent>
{
    public override async Task Handle
    (StaffAccountDeletedDomainEvent domainEvent, 
        CancellationToken cancellationToken = default)
    {

        await bus.PublishAsync(new StaffAccountDeletedIntegrationEvent
            (domainEvent.Id, 
                domainEvent.OccurredOnUtc,
                domainEvent.UserId,
                domainEvent.UniversityId),
            cancellationToken);
    }
}