using MediatR;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.EventBus;
using NextUni.Common.Application.Exceptions;
using NextUni.Common.Application.Messaging;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Application.Abstractions.FormatService;
using NextUni.Modules.Academic.Application.IntroductionBlogs.CreateMajorIntroductionBlog;
using NextUni.Modules.Academic.Domain.Majors;
using NextUni.Modules.Academic.IntegrationEvents;

namespace NextUni.Modules.Academic.Application.Majors.CreateMajor;

public class MajorCreatedDomainEventHandler(ISender sender, IEventBus bus, IAcademicDbContext dbContext, IFormatService formatService): DomainEventHandler<MajorCreatedDomainEvent>
{
    public override async Task Handle(MajorCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // var createIntroductionBlogTask =  sender.Send(new CreateMajorIntroductionBlog.Command(
        //     domainEvent.MajorId, 
        //     domainEvent.Title, 
        //     domainEvent.Content));
        
        var major = await dbContext.Majors
            .Include(m => m.University)
            .FirstOrDefaultAsync(m => m.Id == domainEvent.MajorId, cancellationToken);
        
        if (major is null)
        {
            throw new NextUniException($"University with ID: '{domainEvent.MajorId}' not found.");
        }

        var formattedContentTask = formatService.FormatMajorAsync(
                major.University.Name,
                major.Name,
                major.Code
        ); 

        var publishIntegrationEventTask = formattedContentTask.ContinueWith(async formatted =>
        {
            var integrationEvent = new MajorCreatedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                major.Id,
                formatted.Result);

            await bus.PublishAsync(integrationEvent, cancellationToken);
        }).Unwrap(); 

        await Task.WhenAll(publishIntegrationEventTask);

        // if (createIntroductionBlogTask.Result.IsFailure)
        // {
        //     throw new NextUniException("Failed to create university introduction blog", createIntroductionBlogTask.Result.Error);
        // }
    }
}