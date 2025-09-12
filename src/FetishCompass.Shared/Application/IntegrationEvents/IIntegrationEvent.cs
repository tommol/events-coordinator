namespace FetishCompass.Shared.Application.IntegrationEvents
{
    public interface IIntegrationEvent
    {
        DateTime OccurredOn { get; }
    }
    public abstract record IntegrationEvent : IIntegrationEvent
    {
        protected IntegrationEvent()
        {
            OccurredOn = DateTime.UtcNow;
        }
        public DateTime OccurredOn { get; init; }
    }
}

