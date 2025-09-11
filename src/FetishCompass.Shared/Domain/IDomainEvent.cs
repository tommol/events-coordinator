namespace FetishCompass.Shared.Domain;

public interface IDomainEvent
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
}
