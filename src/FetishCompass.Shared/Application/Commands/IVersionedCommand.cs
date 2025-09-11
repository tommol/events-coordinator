namespace FetishCompass.Shared.Application.Commands;

public interface IVersionedCommand : ICommand
{
    long ExpectedVersion { get; }
}

public interface IVersionedCommand<out TResponse> : ICommand<TResponse>
{
    long ExpectedVersion { get; }
}
