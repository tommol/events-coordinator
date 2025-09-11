using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Shared.Application.Common;

public interface ICommandBus
{
    Task Send(ICommand command, CancellationToken cancellationToken = default);
    Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
}
