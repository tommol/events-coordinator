using FetishCompass.Shared.Domain;

namespace FetishCompass.Shared.Infrastructure.Repository;

public class AggregateRepository<TAggregate, TId> : IAggregateRepository<TAggregate, TId>
    where TAggregate : AggregateRoot<TId>
    where TId : notnull
{
    private readonly IEventStore _eventStore;

    public AggregateRepository(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var events = await _eventStore.LoadEventsAsync(id, cancellationToken);
        if (!events.Any())
        {
            return null;
        }

        var aggregate = (TAggregate?)Activator.CreateInstance(typeof(TAggregate), true);
        if (aggregate is null)
        {
            throw new InvalidOperationException($"Could not create instance of {typeof(TAggregate).FullName}");
        }

        aggregate.LoadFromHistory(events);
        return aggregate;
    }

    public async Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
    {
        var uncommittedEvents = aggregate.UncommittedEvents;
        if (!uncommittedEvents.Any())
        {
            return;
        }

        await _eventStore.AppendEventsAsync(aggregate.Id!, uncommittedEvents, aggregate.Version - uncommittedEvents.Count, cancellationToken);
        aggregate.MarkEventsAsCommitted();
    }
}

