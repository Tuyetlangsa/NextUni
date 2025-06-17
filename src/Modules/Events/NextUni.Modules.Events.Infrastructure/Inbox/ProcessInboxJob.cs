using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NextUni.Common.Application.Clock;
using NextUni.Common.Application.EventBus;
using NextUni.Common.Infrastructure.Inbox;
using NextUni.Common.Infrastructure.Serialization;
using NextUni.Modules.Events.Infrastructure.Database;
using NextUni.Modules.Users.Infrastructure.Inbox;
using Quartz;

namespace NextUni.Modules.Events.Infrastructure.Inbox;

[DisallowConcurrentExecution]
internal sealed class ProcessInboxJob(
    EventDbContext dbContext,
    IServiceScopeFactory serviceScopeFactory,
    IDateTimeProvider dateTimeProvider,
    IOptions<InboxOptions> inboxOptions,
    ILogger<ProcessInboxJob> logger) : IJob
{
    private const string ModuleName = "Events";

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("{Module} - Beginning to process inbox messages", ModuleName);

        IReadOnlyList<InboxMessageResponse> inboxMessages = await GetInboxMessagesAsync();

        foreach (InboxMessageResponse inboxMessage in inboxMessages)
        {
            Exception? exception = null;

            try
            {
                IIntegrationEvent integrationEvent = JsonConvert.DeserializeObject<IIntegrationEvent>(
                    inboxMessage.Content,
                    SerializerSettings.Instance)!;

                using IServiceScope scope = serviceScopeFactory.CreateScope();

                IEnumerable<IIntegrationEventHandler> handlers = IntegrationEventHandlersFactory.GetHandlers(
                    integrationEvent.GetType(),
                    scope.ServiceProvider,
                    Api.AssemblyReference.Assembly);

                foreach (IIntegrationEventHandler integrationEventHandler in handlers)
                {
                    await integrationEventHandler.Handle(integrationEvent, context.CancellationToken);
                }
            }
            catch (Exception caughtException)
            {
                logger.LogError(
                    caughtException,
                    "{Module} - Exception while processing inbox message {MessageId}",
                    ModuleName,
                    inboxMessage.Id);

                exception = caughtException;
            }

            UpdateInboxMessageAsync(inboxMessage, exception);
        }

        await dbContext.SaveChangesAsync();
        
        logger.LogInformation("{Module} - Completed processing inbox messages", ModuleName);
    }

    private async Task<IReadOnlyList<InboxMessageResponse>> GetInboxMessagesAsync()
    {
        return await dbContext
            .Set<InboxMessage>()
            .FromSqlInterpolated($"""
                                  SELECT *
                                  FROM events.inbox_messages
                                  WHERE processed_on_utc IS NULL
                                  ORDER BY occurred_on_utc
                                  LIMIT {inboxOptions.Value.BatchSize}
                                  FOR UPDATE
                                  """)
            .Select(message => new InboxMessageResponse(message.Id, message.Content))
            .ToListAsync();
    }

    private void UpdateInboxMessageAsync(
        InboxMessageResponse inboxMessageResponse,
        Exception? exception)
    {
        var inboxMessage = dbContext
            .Set<InboxMessage>()
            .AsNoTracking()
            .SingleOrDefault(message => message.Id == inboxMessageResponse.Id)!;

        dbContext
            .Set<InboxMessage>()
            .Update(new InboxMessage
            {
                Id = inboxMessage.Id,
                OccurredOnUtc = inboxMessage.OccurredOnUtc,
                Content = inboxMessage.Content,
                Type = inboxMessage.Type,
                ProcessedOnUtc = dateTimeProvider.UtcNow,
                Error = exception?.ToString(),
            });
    }

    internal sealed record InboxMessageResponse(Guid Id, string Content);
}
