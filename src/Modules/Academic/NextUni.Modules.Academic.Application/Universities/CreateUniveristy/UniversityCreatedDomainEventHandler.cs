using MediatR;
using NextUni.Common.Application.EventBus;
using NextUni.Common.Application.Exceptions;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Application.Abstractions.FormatService;
using NextUni.Modules.Academic.Application.IntroductionBlogs.CreateUniversityIntroductionBlog;
using NextUni.Modules.Academic.Domain.Universities;
using NextUni.Modules.Academic.IntegrationEvents;

namespace NextUni.Modules.Academic.Application.Universities.CreateUniveristy;

public class UniversityCreatedDomainEventHandler(ISender sender, IEventBus bus, IAcademicDbContext dbContext, IFormatService formatService): DomainEventHandler<UniversityCreatedDomainEvent>
{
    public override async Task Handle(UniversityCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // var result = await sender.Send(new CreateUniversityIntroductionBlog.Command(
        //     domainEvent.UniversityId, 
        //     domainEvent.Title, 
        //     domainEvent.Content));
        //
        // if (result.IsFailure)
        // {
        //     throw new NextUniException("Handle UniversityCreatedDomainEvent failed", result.Error);
        // }
        //
        // var university = await dbContext.Universities
        //     .FindAsync( domainEvent.UniversityId , cancellationToken);
        // if (university is null)
        // {
        //     throw new NextUniException("Handle UniversityCreatedDomainEvent failed", result.Error);
        // }
        //
        // var formattedContent = await formatService.FormateUniversityAsync(
        //     university.Name,
        //     university.Region.ToString(),
        //     university.UniversityType.ToString(),
        //     university.Address,
        //     university.Email,
        //     university.WebsiteUrl,
        //     university.FacebookUrl);
        //
        // var intgrationEvent = new UniversityCreatedIntegrationEvent(
        //     domainEvent.Id, 
        //     domainEvent.OccurredOnUtc, 
        //     domainEvent.UniversityId, 
        //     formattedContent);
        //
        // await bus.PublishAsync(intgrationEvent, cancellationToken);
        
        
        var university = await dbContext.Universities.FindAsync(domainEvent.UniversityId, cancellationToken);
        if (university is null)
        {
            throw new NextUniException($"University with ID: '{domainEvent.UniversityId}' not found.");
        }

        var formattedContentTask = formatService.FormateUniversityAsync(
            university.Name,
            university.Region.ToString(),
            university.UniversityType.ToString(),
            university.Address,
            university.Email,
            university.WebsiteUrl,
            university.FacebookUrl
        );

        // var createBlogTask = sender.Send(new CreateUniversityIntroductionBlog.Command(
        //     university.Id,
        //     domainEvent.Title,
        //     domainEvent.Content), cancellationToken);

        var publishIntegrationEventTask = formattedContentTask.ContinueWith(async formatted =>
        {
            var integrationEvent = new UniversityCreatedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                university.Id,
                formatted.Result);

            await bus.PublishAsync(integrationEvent, cancellationToken);
        }).Unwrap(); 

        await Task.WhenAll( publishIntegrationEventTask);

        // if (createBlogTask.Result.IsFailure)
        // {
        //     throw new NextUniException("Failed to create university introduction blog", createBlogTask.Result.Error);
        // }
        
    }
}