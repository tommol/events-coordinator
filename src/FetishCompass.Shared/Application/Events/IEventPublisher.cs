using FetishCompass.Shared.Domain;

namespace FetishCompass.Shared.Application.Events;

public interface IEventPublisher
{
    Task PublishDomainEventsAsync<TId>(Entity<TId> entity, CancellationToken cancellationToken = default)
        where TId : notnull;

    Task PublishDomainEventsAsync<TId>(AggregateRoot<TId> aggregateRoot, CancellationToken cancellationToken = default)
        where TId : notnull;

    Task PublishDomainEventsAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
