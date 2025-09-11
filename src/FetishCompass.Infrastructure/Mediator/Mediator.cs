using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Application.Queries;
using FetishCompass.Shared.Application.Common;
using ICommand = FetishCompass.Shared.Application.Commands.ICommand;

namespace FetishCompass.Infrastructure.Mediator
{
   
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _provider;

        public Mediator(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public Task Send(ICommand command, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            var handler = _provider.GetService(handlerType);
            if (handler == null)
                throw new InvalidOperationException($"No handler registered for command type {command.GetType().FullName}");
            
            return ((dynamic)handler).Handle((dynamic)command, cancellationToken);
        }

        public Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
            var handler = _provider.GetService(handlerType);
            if (handler == null)
                throw new InvalidOperationException($"No handler registered for command type {command.GetType().FullName} returning {typeof(TResponse).FullName}");

            return ((dynamic)handler).Handle((dynamic)command, cancellationToken);
        }

        public Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
            var handler = _provider.GetService(handlerType);
            if (handler == null)
                throw new InvalidOperationException($"No handler registered for query type {query.GetType().FullName} returning {typeof(TResponse).FullName}");

            return ((dynamic)handler).Handle((dynamic)query, cancellationToken);
        }
    }
}
