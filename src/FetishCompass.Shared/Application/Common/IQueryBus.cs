using FetishCompass.Shared.Application.Queries;

namespace FetishCompass.Shared.Application.Common;

public interface IQueryBus
{
    Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
}
