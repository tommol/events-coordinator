namespace FetishCompass.Shared.Application.Events;

public interface IEventStore
{
    Task SaveAsync(IIntegrationEvent @event, CancellationToken cancellationToken = default);
    Task SaveManyAsync(IEnumerable<IIntegrationEvent> events, CancellationToken cancellationToken = default);
    Task<IEnumerable<IIntegrationEvent>> GetUnpublishedEventsAsync(CancellationToken cancellationToken = default);
    Task MarkAsPublishedAsync(Guid eventId, CancellationToken cancellationToken = default);
    Task<IEnumerable<IIntegrationEvent>> GetEventsByAggregateIdAsync(Guid aggregateId, CancellationToken cancellationToken = default);
}
