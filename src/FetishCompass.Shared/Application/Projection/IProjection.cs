namespace FetishCompass.Shared.Application.Projection;

public interface IProjection
{
    string Name { get; }                
    Task HandleAsync(object @event, EventMetadata meta, CancellationToken ct);
}