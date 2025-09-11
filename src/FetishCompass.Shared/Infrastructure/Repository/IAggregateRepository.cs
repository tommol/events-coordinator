using FetishCompass.Shared.Domain;

namespace FetishCompass.Shared.Infrastructure.Repository;

public interface IAggregateRepository<TAggregate, TId>
    where TAggregate : AggregateRoot<TId>
    where TId : notnull
{
    Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task<TAggregate?> GetByIdAndVersionAsync(TId id, long version, CancellationToken cancellationToken = default);

    Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
    Task SaveAsync(TAggregate aggregate, long expectedVersion, CancellationToken cancellationToken = default);

    Task DeleteAsync(TId id, long expectedVersion, CancellationToken cancellationToken = default);
}
