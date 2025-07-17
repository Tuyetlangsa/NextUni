using MediatR;
using NextUni.Common.Application.EventBus;
using NextUni.Common.Application.Exceptions;
using NextUni.Modules.Academic.IntegrationEvents;
using NextUni.Modules.Chatbot.Application.Majors;

namespace NextUni.Modules.Chatbot.Api.Universities;

public class MajorCreatedIntegrationEventHandler(ISender sender) : IntegrationEventHandler<MajorCreatedIntegrationEvent>
{
    public override async Task Handle(MajorCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new CreateMajor.Command(
                integrationEvent.MajorId,
                integrationEvent.TextFormatted),
            cancellationToken);
        
        if (result.IsFailure)
        {
            throw new NextUniException(nameof(CreateMajor.Command), result.Error);
        }
    }
}