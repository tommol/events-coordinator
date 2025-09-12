namespace FetishCompass.Shared.Application.IntegrationEvents;

public interface IIntegrationEventHandler<TEvent> where TEvent : IntegrationEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}