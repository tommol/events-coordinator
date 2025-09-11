using FetishCompass.Shared.Domain;
using FetishCompass.Shared.Domain.Exceptions;

namespace FetishCompass.Shared.Application.Common;

public static class OptimisticLockingExtensions
{
    public static void EnsureVersion<TId>(this AggregateRoot<TId> aggregate, long expectedVersion)
        where TId : notnull
    {
        aggregate.CheckVersion(expectedVersion);
    }

    public static void EnsureVersion<TId>(this Entity<TId> entity, long expectedVersion)
        where TId : notnull
    {
        entity.CheckVersion(expectedVersion);
    }

    public static Result CheckVersionSafe<TId>(this AggregateRoot<TId> aggregate, long expectedVersion)
        where TId : notnull
    {
        try
        {
            aggregate.CheckVersion(expectedVersion);
            return Result.Success();
        }
        catch (ConcurrencyException ex)
        {
            return Result.Failure(Error.Conflict("Concurrency.Conflict", ex.Message));
        }
    }

    public static Result CheckVersionSafe<TId>(this Entity<TId> entity, long expectedVersion)
        where TId : notnull
    {
        try
        {
            entity.CheckVersion(expectedVersion);
            return Result.Success();
        }
        catch (ConcurrencyException ex)
        {
            return Result.Failure(Error.Conflict("Concurrency.Conflict", ex.Message));
        }
    }
}
