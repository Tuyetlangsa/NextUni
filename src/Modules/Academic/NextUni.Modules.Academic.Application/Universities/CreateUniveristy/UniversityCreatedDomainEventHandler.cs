using MediatR;
using NextUni.Common.Application.Exceptions;
using NextUni.Common.Application.Messaging;
using NextUni.Modules.Academic.Application.IntroductionBlogs.CreateUniversityIntroductionBlog;
using NextUni.Modules.Academic.Domain.Universities;

namespace NextUni.Modules.Academic.Application.Universities.CreateUniveristy;

public class UniversityCreatedDomainEventHandler(ISender sender): DomainEventHandler<UniversityCreatedDomainEvent>
{
    public override async Task Handle(UniversityCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new CreateUniversityIntroductionBlog.Command(
            domainEvent.UniversityId, 
            domainEvent.Title, 
            domainEvent.Content));

        if (result.IsFailure)
        {
            throw new NextUniException("Handle UniversityCreatedDomainEvent failed", result.Error);
        }
    }
}