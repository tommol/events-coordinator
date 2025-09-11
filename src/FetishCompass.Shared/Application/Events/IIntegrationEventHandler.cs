namespace FetishCompass.Shared.Application.Events;

public interface IIntegrationEventHandler<in TEvent>
    where TEvent : IIntegrationEvent
{
    Task Handle(TEvent @event, CancellationToken cancellationToken = default);
}

public interface IIntegrationEventHandler
{
    Task Handle(IIntegrationEvent @event, CancellationToken cancellationToken = default);
}
