namespace FetishCompass.Shared.Application.Events;

public interface IIntegrationEvent
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
    string EventType { get; }
    int Version { get; }
}
