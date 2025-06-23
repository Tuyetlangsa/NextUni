using MediatR;
using NextUni.Common.Application.Exceptions;
using NextUni.Common.Application.Messaging;
using NextUni.Modules.Academic.Application.IntroductionBlogs.UpdateMajorIntroductionBlog;
using NextUni.Modules.Academic.Domain.Majors;

namespace NextUni.Modules.Academic.Application.Majors.UpdateMajor
{
    public class MajorUpdatedDomainEventHandler(ISender sender) : DomainEventHandler<MajorUpdatedDomainEvent>
    {
        public override async Task Handle(MajorUpdatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var result = await sender.Send(new UpdateMajorIntroductionBlog.Command(
                domainEvent.MajorId,
                domainEvent.Id,
                domainEvent.Title,
                domainEvent.Content));

            if (result.IsFailure)
            {
                throw new NextUniException("Handle MajorUpdatedDomainEvent failed", result.Error);
            }
        }
    }
}
