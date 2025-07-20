using MediatR;
using NextUni.Common.Application.EventBus;
using NextUni.Common.Application.Exceptions;
using NextUni.Modules.Academic.IntegrationEvents;
using NextUni.Modules.Chatbot.Application.AdmissionScoreByYear;

namespace NextUni.Modules.Chatbot.Api.Universities;

public class AdmissionScoreIntegrationEventHandler(ISender sender) : IntegrationEventHandler<AdmissionScoreByYearCreatedIntegrationEvent>
{
    public override async Task Handle(AdmissionScoreByYearCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new CreateAdmissionScoreByYear.Command(
                integrationEvent.UniversityId,
                integrationEvent.TextFormatted),
            cancellationToken);
        
        if (result.IsFailure)
        {
            throw new NextUniException(nameof(CreateAdmissionScoreByYear.Command), result.Error);
        }
    }
}