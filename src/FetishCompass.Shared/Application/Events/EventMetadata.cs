namespace FetishCompass.Shared.Application.Events;

public record EventMetadata(
    string Source,
    string CorrelationId,
    string? CausationId = null,
    string? UserId = null,
    Dictionary<string, object>? Properties = null)
{
    public static EventMetadata Create(string source, string? correlationId = null, string? causationId = null, string? userId = null)
    {
        return new EventMetadata(
            source,
            correlationId ?? Guid.NewGuid().ToString(),
            causationId,
            userId);
    }
}

public record EnvelopedEvent<TEvent>(
    TEvent Event,
    EventMetadata Metadata) where TEvent : IIntegrationEvent;
