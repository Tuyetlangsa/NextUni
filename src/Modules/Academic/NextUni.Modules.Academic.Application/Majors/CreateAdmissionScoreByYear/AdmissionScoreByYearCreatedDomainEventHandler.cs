using MediatR;
using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.EventBus;
using NextUni.Common.Application.Exceptions;
using NextUni.Common.Application.Messaging;
using NextUni.Modules.Academic.Application.Abstractions.Data;
using NextUni.Modules.Academic.Application.Abstractions.FormatService;
using NextUni.Modules.Academic.Domain.Majors;
using NextUni.Modules.Academic.IntegrationEvents;

namespace NextUni.Modules.Academic.Application.Majors.CreateAdmissionScoreByYear;

public class AdmissionScoreByYearCreatedDomainEventHandler(
    ISender sender, 
    IEventBus bus, 
    IAcademicDbContext dbContext, 
    IFormatService formatService)
    : DomainEventHandler<AdmissionScoreByYearCreatedDomainEvent>
{
    public override async Task Handle(AdmissionScoreByYearCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var university = await dbContext.Universities
            .Include(u => u.Majors)
            .ThenInclude(m => m.AdmissionScore.Where(a => a.Year.Year == domainEvent.Year))
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == domainEvent.UniversityId, cancellationToken);
        
        if (university is null)
        {
            throw new NextUniException($"University with ID: '{domainEvent.UniversityId}' not found.");
        }

        string formattedText = await formatService.FormatAdmissionScoresAsync(
            university.Name, 
            university.Majors, 
            domainEvent.Year);
        
        var integrationEvent = new AdmissionScoreByYearCreatedIntegrationEvent(
            domainEvent.Id,
            domainEvent.OccurredOnUtc,
            domainEvent.UniversityId,
            formattedText);       
        
        await bus.PublishAsync(integrationEvent, cancellationToken);
    }
}