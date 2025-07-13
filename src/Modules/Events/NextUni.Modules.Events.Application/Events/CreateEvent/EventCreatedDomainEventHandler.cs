using MediatR;
using NextUni.Common.Application.Exceptions;
using NextUni.Common.Application.Messaging;
using NextUni.Modules.Events.Application.IntroductionBlogs;
using NextUni.Modules.Events.Domain.Events;

namespace NextUni.Modules.Events.Application.Events.CreateEvent;

public class EventCreatedDomainEventHandler(ISender sender): DomainEventHandler<EventCreatedDomainEvent>
{
    public override async Task Handle(EventCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new CreateEventIntroductionBlog.Command(
            domainEvent.EventId, 
            domainEvent.Title, 
            domainEvent.Content));

        if (result.IsFailure)
        {
            throw new NextUniException("Handle EventCreatedDomainEvent failed", result.Error);
        }
    }
}