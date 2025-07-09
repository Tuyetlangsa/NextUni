using MediatR;
using NextUni.Common.Application.EventBus;
using NextUni.Common.Application.Exceptions;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Universities.AssignStaffAccount;
using NextUni.Modules.Users.IntegrationEvents;

namespace NextUni.Modules.Academic.Api.Universities;



public class StaffAccountCreatedIntegrationDomainEventHandler(ISender sender) : IntegrationEventHandler<StaffAccountCreatedIntegrationEvent>
{
    public override async Task Handle(StaffAccountCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new AssignStaffAccount.Command(
                integrationEvent.UserId,
                integrationEvent.UniversityId
                ),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new NextUniException(nameof(AssignStaffAccount.Command), result.Error);
        }
    }
}