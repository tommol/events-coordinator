using FetishCompass.Shared.Application.Projection;

namespace FetishCompass.Shared.Infrastructure.Projection;

public interface IEventSource
{
    IAsyncEnumerable<(object Event, EventMetadata Meta)> 
        ReadFromAsync(ulong start, CancellationToken ct);
}