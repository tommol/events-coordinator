using FetishCompass.Shared.Domain.Exceptions;

namespace FetishCompass.Shared.Domain;

public abstract class AggregateRoot<TId> : IEquatable<AggregateRoot<TId>>, IVersioned
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();
    // Event Sourcing: lista niezatwierdzonych zdarzeń
    private readonly List<IDomainEvent> _uncommittedEvents = new();

    protected AggregateRoot(TId id)
    {
        Id = id;
        Version = 0;
    }

    protected AggregateRoot()
    {
        // Dla Entity Framework
        Version = 0;
    }

    public TId Id { get; protected set; } = default!;

    public long Version { get; private set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => this._domainEvents.AsReadOnly();
    public IReadOnlyCollection<IDomainEvent> UncommittedEvents => _uncommittedEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        this._domainEvents.Add(domainEvent);
    }

    protected void ApplyChange(IDomainEvent @event, bool isNew = true)
    {
        var method = this.GetType().GetMethod("Apply", new[] { @event.GetType() });
        if (method == null)
            throw new InvalidOperationException($"Apply method not found for {@event.GetType().Name}");
        method.Invoke(this, new object[] { @event });
        if (isNew)
        {
            _uncommittedEvents.Add(@event);
            AddDomainEvent(@event);
        }
    }

    public void ClearDomainEvents()
    {
        this._domainEvents.Clear();
    }

    public void IncrementVersion()
    {
        Version++;
    }

    public void CheckVersion(long expectedVersion)
    {
        if (Version != expectedVersion)
        {
            throw ConcurrencyException.CreateForAggregate(Id, expectedVersion, Version);
        }
    }

    protected void MarkAsModified()
    {
        IncrementVersion();
    }

    public void LoadFromHistory(IEnumerable<IDomainEvent> history)
    {
        foreach (var e in history)
        {
            ApplyChange(e, false);
            Version++;
        }
        _uncommittedEvents.Clear();
    }

    public void MarkEventsAsCommitted()
    {
        _uncommittedEvents.Clear();
    }

    public bool Equals(AggregateRoot<TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return EqualityComparer<TId>.Default.Equals(Id, other.Id) && GetType() == other.GetType();
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as AggregateRoot<TId>);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, GetType());
    }

    public static bool operator ==(AggregateRoot<TId>? left, AggregateRoot<TId>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(AggregateRoot<TId>? left, AggregateRoot<TId>? right)
    {
        return !Equals(left, right);
    }
}