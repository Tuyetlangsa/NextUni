using MediatR;
using NextUni.Common.Application.EventBus;
using NextUni.Common.Application.Exceptions;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Users.Application.Users.GetUser;
using NextUni.Modules.Users.Domain.Users;
using NextUni.Modules.Users.IntegrationEvents;

namespace NextUni.Modules.Users.Application.Users.RegisterUser;

public class UserRegistedDomainEventHandler(ISender sender, IEventBus bus) : DomainEventHandler<UserRegisteredDomainEvent>
{
    public override async Task Handle
        (UserRegisteredDomainEvent domainEvent, 
        CancellationToken cancellationToken = default)
    {
        Result<UserResponse>? user = await sender.Send(
            new GetUserQuery(domainEvent.UserId),
            cancellationToken);
        if (user.IsFailure)
        {
            throw new NextUniException(nameof(GetUserQuery), user.Error);
        }

        await bus.PublishAsync(new UserRegistedIntegrationEvent
            (domainEvent.Id, 
            domainEvent.OccurredOnUtc,
            user.Value.Id,
            user.Value.Email,
            user.Value.FirstName,
            user.Value.LastName,
            user.Value.PhoneNumber),
            cancellationToken);
    }
}