using MediatR;
using NextUni.Common.Application.Exceptions;
using NextUni.Common.Application.Messaging;
using NextUni.Modules.Academic.Application.IntroductionBlogs.CreateMajorIntroductionBlog;
using NextUni.Modules.Academic.Domain.Majors;

namespace NextUni.Modules.Academic.Application.Majors.CreateMajor;

public class MajorCreatedDomainEventHandler(ISender sender): DomainEventHandler<MajorCreatedDomainEvent>
{
    public override async Task Handle(MajorCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new CreateMajorIntroductionBlog.Command(
            domainEvent.MajorId, 
            domainEvent.Title, 
            domainEvent.Content));

        if (result.IsFailure)
        {
            throw new NextUniException("Handle MajorCreatedDomainEvent failed", result.Error);
        }
    }
}