namespace FetishCompass.Shared.Domain.Exceptions;

public class ConcurrencyException : DomainException
{
    public ConcurrencyException(string message) : base(message)
    {
    }

    public ConcurrencyException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static ConcurrencyException Create<TId>(TId id, long expectedVersion, long actualVersion)
        where TId : notnull
    {
        return new ConcurrencyException(
            $"Concurrency conflict detected for entity with ID '{id}'. " +
            $"Expected version: {expectedVersion}, but actual version is: {actualVersion}");
    }

    public static ConcurrencyException CreateForAggregate<TId>(TId aggregateId, long expectedVersion, long actualVersion)
        where TId : notnull
    {
        return new ConcurrencyException(
            $"Concurrency conflict detected for aggregate with ID '{aggregateId}'. " +
            $"Expected version: {expectedVersion}, but actual version is: {actualVersion}");
    }
}
