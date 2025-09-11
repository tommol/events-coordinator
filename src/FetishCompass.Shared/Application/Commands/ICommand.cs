namespace FetishCompass.Shared.Application.Commands;

public interface ICommand
{
}

public interface ICommand<out TResponse> : ICommand
{
}
