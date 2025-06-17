using MassTransit;
using Newtonsoft.Json;
using NextUni.Common.Application.EventBus;
using NextUni.Common.Infrastructure.Inbox;
using NextUni.Common.Infrastructure.Serialization;
using NextUni.Modules.Academic.Infrastructure.Database;

namespace NextUni.Modules.Academic.Infrastructure.Inbox;

internal sealed class IntegrationEventConsumer<TIntegrationEvent>(AcademicDbContext dbContext)
    : IConsumer<TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent
{
    public async Task Consume(ConsumeContext<TIntegrationEvent> context)
    {
        TIntegrationEvent integrationEvent = context.Message;

        var inboxMessage = new InboxMessage()
        {
            Id = integrationEvent.Id,
            Type = integrationEvent.GetType().Name,
            Content = JsonConvert.SerializeObject(integrationEvent, SerializerSettings.Instance),
            OccurredOnUtc = integrationEvent.OccurredOnUtc
        };

        await dbContext
            .Set<InboxMessage>()
            .AddAsync(inboxMessage);
        
        await dbContext.SaveChangesAsync();
    }
}
