using System.Threading.Tasks;

namespace FetishCompass.Shared.Application.IntegrationEvents
{
    public interface IIntegrationEventPublisher
    {
        Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
        Task PublishAsync(IEnumerable<IIntegrationEvent> integrationEvents, CancellationToken cancellationToken = default);
    }
}

