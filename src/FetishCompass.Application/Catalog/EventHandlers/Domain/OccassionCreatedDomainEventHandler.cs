using FetishCompass.Application.Catalog.IntegrationEvents;
using FetishCompass.Domain.Catalog.DomainEvents;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Application.IntegrationEvents;
using FetishCompass.Shared.Domain;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Catalog.EventHandlers.Domain;

public sealed class OccasionCreatedDomainEventHandler(
    IAggregateRepository<Occasion, OccasionId> repository,
    IIntegrationEventPublisher publisher,
    ILogger<OccasionCreatedDomainEventHandler> logger)
    : IDomainEventHandler<OccasionCreatedDomainEvent>
{

    public async Task HandleAsync(OccasionCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            var occasion = await repository.GetByIdAsync(domainEvent.OccasionId, cancellationToken);
            if (occasion == null)
            {
                throw new InvalidOperationException($"Occasion with ID {domainEvent.OccasionId} not found.");
            }
            var integrationEvent = new OccasionCreatedIntegrationEvent(
                domainEvent.OccasionId.Value,
                domainEvent.Title,
                domainEvent.Description,
                domainEvent.OccasionStart.ToUtcInstant().LocalDateTime,
                domainEvent.OccasionEnd.ToUtcInstant().LocalDateTime,
                domainEvent.OccasionStart.TimeZoneId,
                domainEvent.Organizer.Value,
                domainEvent.Venue.Value);
            await publisher.PublishAsync(integrationEvent, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw;
        }
    }
}
