using MediatR;
using NextUni.Common.Application.Exceptions;
using NextUni.Common.Application.Messaging;
using NextUni.Modules.Academic.Application.IntroductionBlogs.UpdateUniversityIntroductionBlog;
using NextUni.Modules.Academic.Domain.Universities;

namespace NextUni.Modules.Academic.Application.Universities.UpdateUniversity;

public class UniversityUpdatedDomainEventHandler(ISender sender): DomainEventHandler<UniversityUpdatedDomainEvent>
{
    public override async Task Handle(UniversityUpdatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new UpdateUniversityIntroductionBlog.Command(
            domainEvent.UniversityId,
            domainEvent.Title,
            domainEvent.Content));

        if (result.IsFailure)
        {
            throw new NextUniException("Handle UniversityUpdatedDomainEvent failed", result.Error);
        }
    }
}