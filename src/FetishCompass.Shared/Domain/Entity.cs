namespace FetishCompass.Shared.Domain;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{

    protected Entity(TId id)
    {
        Id = id;
    }

    protected Entity()
    {
    }

    public TId Id { get; protected set; } = default!;
    
    
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
