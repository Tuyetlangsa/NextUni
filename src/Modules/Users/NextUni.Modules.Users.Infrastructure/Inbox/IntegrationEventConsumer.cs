using MassTransit;
using Newtonsoft.Json;
using NextUni.Common.Application.EventBus;
using NextUni.Common.Infrastructure.Inbox;
using NextUni.Common.Infrastructure.Serialization;
using NextUni.Modules.Users.Infrastructure.Database;

namespace NextUni.Modules.Users.Infrastructure.Inbox;

internal sealed class IntegrationEventConsumer<TIntegrationEvent>(UserDbContext dbContext)
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
