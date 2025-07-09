using MediatR;
using NextUni.Common.Application.EventBus;
using NextUni.Common.Application.Exceptions;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Users.Application.Users.GetUser;
using NextUni.Modules.Users.Domain.Users;
using NextUni.Modules.Users.IntegrationEvents;

namespace NextUni.Modules.Users.Application.Staffs.CreateStaffAccount;


public class StaffAccountCreatedDomainEventHandler(ISender sender, IEventBus bus) : DomainEventHandler<StaffAccountCreatedDomainEvent>
{
    public override async Task Handle
    (StaffAccountCreatedDomainEvent domainEvent, 
        CancellationToken cancellationToken = default)
    {
        Result<UserResponse>? user = await sender.Send(
            new GetUserQuery(domainEvent.UserId),
            cancellationToken);
        if (user.IsFailure)
        {
            throw new NextUniException(nameof(GetUserQuery), user.Error);
        }

        await bus.PublishAsync(new StaffAccountCreatedIntegrationEvent
            (domainEvent.Id, 
                domainEvent.OccurredOnUtc,
                user.Value.Id,
                domainEvent.UniversityId),
            cancellationToken);
    }
}