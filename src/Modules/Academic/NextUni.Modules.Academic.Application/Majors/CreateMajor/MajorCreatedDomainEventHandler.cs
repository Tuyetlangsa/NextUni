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
        var university = await dbContext.Universities
            .Include(u => u.Majors)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == domainEvent.UniversityId, cancellationToken);
        
        if (university is null)
        {
            throw new NextUniException($"University with ID: '{domainEvent.UniversityId}' not found.");
        }

        var listMajorNames = university.Majors
            .Select(m => m.Name)
            .ToList();
        
        var formattedContentTask = formatService.FormatMajorAsync(
            domainEvent.UniversityId,
            university.Name,
            listMajorNames
        ); 

        var publishIntegrationEventTask = formattedContentTask.ContinueWith(async formatted =>
        {
            var integrationEvent = new MajorCreatedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.UniversityId,
                formatted.Result);

            await bus.PublishAsync(integrationEvent, cancellationToken);
        }).Unwrap(); 

        await Task.WhenAll(publishIntegrationEventTask);
    }
}