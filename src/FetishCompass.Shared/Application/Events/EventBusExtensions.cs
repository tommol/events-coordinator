namespace FetishCompass.Shared.Application.Events;

public static class EventBusExtensions
{
    public static async Task PublishWithMetadataAsync<TEvent>(
        this IEventBus eventBus,
        TEvent @event,
        EventMetadata metadata,
        CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent
    {
        var envelopedEvent = new EnvelopedEvent<TEvent>(@event, metadata);
        await eventBus.PublishAsync(@event, cancellationToken);
    }

    public static async Task PublishFromSourceAsync<TEvent>(
        this IEventBus eventBus,
        TEvent @event,
        string source,
        string? userId = null,
        CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent
    {
        var metadata = EventMetadata.Create(source, userId: userId);
        await eventBus.PublishWithMetadataAsync(@event, metadata, cancellationToken);
    }
}
