using FetishCompass.Shared.Domain.Exceptions;

namespace FetishCompass.Shared.Domain;

public abstract class Entity<TId> : IEquatable<Entity<TId>>, IVersioned
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected Entity(TId id)
    {
        Id = id;
        Version = 0;
    }

    protected Entity()
    {
        // Dla Entity Framework
        Version = 0;
    }

    public TId Id { get; protected set; } = default!;

    public long Version { get; private set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void IncrementVersion()
    {
        Version++;
    }

    public void CheckVersion(long expectedVersion)
    {
        if (Version != expectedVersion)
        {
            throw ConcurrencyException.Create(Id, expectedVersion, Version);
        }
    }

    protected void MarkAsModified()
    {
        IncrementVersion();
    }

    public bool Equals(Entity<TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return EqualityComparer<TId>.Default.Equals(Id, other.Id) && GetType() == other.GetType();
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Entity<TId>);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, GetType());
    }

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !Equals(left, right);
    }
}
