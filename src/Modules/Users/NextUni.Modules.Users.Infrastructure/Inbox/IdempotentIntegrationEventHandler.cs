using NextUni.Common.Application.EventBus;
using NextUni.Common.Infrastructure.Inbox;
using NextUni.Modules.Users.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NextUni.Modules.Users.Infrastructure.Database;

namespace NextUni.Modules.Users.Infrastructure.Inbox;

internal sealed class IdempotentIntegrationEventHandler<TIntegrationEvent>(
    IIntegrationEventHandler decorated,
    UserDbContext dbContext)
    : IntegrationEventHandler<TIntegrationEvent>
    where TIntegrationEvent : IIntegrationEvent
{
    public override async Task Handle(
        TIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        var inboxMessageConsumer = new InboxMessageConsumer(integrationEvent.Id, decorated.GetType().Name);

        if (await InboxConsumerExistsAsync(inboxMessageConsumer))
        {
            return;
        }

        await decorated.Handle(integrationEvent, cancellationToken);

        await InsertInboxConsumerAsync(inboxMessageConsumer);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private Task<bool> InboxConsumerExistsAsync(InboxMessageConsumer inboxMessageConsumer)
    {
        return dbContext
            .Set<InboxMessageConsumer>()
            .AnyAsync(c => c.InboxMessageId == inboxMessageConsumer.InboxMessageId && c.Name == inboxMessageConsumer.Name);
    }

    private async Task InsertInboxConsumerAsync(InboxMessageConsumer inboxMessageConsumer)
    {
        await dbContext.Set<InboxMessageConsumer>().AddAsync(inboxMessageConsumer);
    }
}
