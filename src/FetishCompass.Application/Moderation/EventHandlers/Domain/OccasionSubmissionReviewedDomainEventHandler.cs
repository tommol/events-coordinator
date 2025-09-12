using FetishCompass.Application.Moderation.IntegrationEvents;
using FetishCompass.Domain.Moderation.DomainEvents;
using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Application.IntegrationEvents;
using FetishCompass.Shared.Domain;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Moderation.EventHandlers.Domain;

public sealed class OccasionSubmissionReviewedDomainEventHandler(
    IIntegrationEventPublisher publisher,
    ILogger<OccasionSubmissionReviewedDomainEventHandler> logger)
    : IDomainEventHandler<OccasionSubmissionReviewedDomainEvent>
{
    public async Task HandleAsync(OccasionSubmissionReviewedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var integrationEvent = new OccasionSubmissionReviewedIntegrationEvent(
                domainEvent.OccasionSubmissionId,
                domainEvent.OccasionId,
                domainEvent.NewStatus == SubmissionStatus.Accepted,
                domainEvent.ReviewedAt);
            await publisher.PublishAsync(integrationEvent, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw;
        }
    }
}