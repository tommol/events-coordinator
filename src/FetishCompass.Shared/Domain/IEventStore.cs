using FetishCompass.Shared.Domain;

namespace FetishCompass.Shared.Domain;

public interface IEventStore
{
    Task<IEnumerable<IDomainEvent>> LoadEventsAsync(object aggregateId, CancellationToken cancellationToken = default);
    Task AppendEventsAsync(object aggregateId, IEnumerable<IDomainEvent> events, long expectedVersion, CancellationToken cancellationToken = default);
}

