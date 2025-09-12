namespace FetishCompass.Shared.Application.IntegrationEvents;

public abstract record IntegrationEvent : IIntegrationEvent
{
    protected IntegrationEvent()
    {
        OccurredOn = DateTime.UtcNow;
    }
    public DateTime OccurredOn { get; init; }
}