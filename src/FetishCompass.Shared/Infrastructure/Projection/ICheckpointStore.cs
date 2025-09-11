namespace FetishCompass.Shared.Infrastructure.Projection;

public interface ICheckpointStore
{
    Task<ulong?> GetAsync(string projectionName, CancellationToken ct);
    Task SaveAsync(string projectionName, ulong position, CancellationToken ct);
}