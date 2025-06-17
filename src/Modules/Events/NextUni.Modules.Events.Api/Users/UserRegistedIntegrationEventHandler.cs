using MediatR;
using NextUni.Common.Application.EventBus;
using NextUni.Common.Application.Exceptions;
using NextUni.Common.Domain;
using NextUni.Modules.Events.Application.Users;
using NextUni.Modules.Users.IntegrationEvents;

namespace NextUni.Modules.Events.Api.Users;

public class UserRegistedIntegrationEventHandler(ISender sender) : IntegrationEventHandler<UserRegistedIntegrationEvent>
{
    public override async Task Handle(UserRegistedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new CreateUser.Command(
                integrationEvent.UserId,
                integrationEvent.Email,
                integrationEvent.FirstName,
                integrationEvent.LastName,
                integrationEvent.PhoneNumber),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new NextUniException(nameof(CreateUser.Command), result.Error);
        }
    }
}