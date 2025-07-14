using MediatR;
using NextUni.Common.Application.EventBus;
using NextUni.Common.Application.Exceptions;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Universities.DeleteStaffAccount;
using NextUni.Modules.Users.IntegrationEvents;

namespace NextUni.Modules.Academic.Api.Universities;


public class StaffAccountDeletedIntegrationEventHandler(ISender sender) : IntegrationEventHandler<StaffAccountDeletedIntegrationEvent>
{
    public override async Task Handle(StaffAccountDeletedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new DeleteStaffAccount.Command(
                integrationEvent.UserId,
                integrationEvent.UniversityId
            ),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new NextUniException(nameof(DeleteStaffAccount.Command), result.Error);
        }
    }
}