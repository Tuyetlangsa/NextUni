using MediatR;
using NextUni.Common.Application.EventBus;
using NextUni.Common.Application.Exceptions;
using NextUni.Modules.Academic.IntegrationEvents;
using NextUni.Modules.Chatbot.Application.Universities;

namespace NextUni.Modules.Chatbot.Api.Universities;

public class UniversityCreatedIntegrationEventHandler(ISender sender) : IntegrationEventHandler<UniversityCreatedIntegrationEvent>
{
    public override async Task Handle(UniversityCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new CreateUniversity.Command(
                integrationEvent.UniversityId,
                integrationEvent.TextFormatted),
            cancellationToken);
        
        if (result.IsFailure)
        {
            throw new NextUniException(nameof(CreateUniversity.Command), result.Error);
        }
    }
}