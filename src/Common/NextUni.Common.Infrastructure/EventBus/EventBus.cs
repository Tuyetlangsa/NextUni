using MassTransit;
using NextUni.Common.Application.EventBus;

namespace NextUni.Common.Infrastructure.EventBus;

public class EventBus(IBus bus) : IEventBus
{
    public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : IIntegrationEvent
    {
        await bus.Publish(integrationEvent, cancellationToken);
    }
}