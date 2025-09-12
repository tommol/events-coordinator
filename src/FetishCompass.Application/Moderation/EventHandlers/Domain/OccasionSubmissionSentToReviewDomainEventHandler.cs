using FetishCompass.Application.Moderation.IntegrationEvents;
using FetishCompass.Domain.Moderation.DomainEvents;
using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Application.IntegrationEvents;
using FetishCompass.Shared.Domain;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Moderation.EventHandlers.Domain;

public sealed class OccasionSubmissionSentToReviewDomainEventHandler(
    IAggregateRepository<ModerationCase, ModerationCaseId> repository,
    IIntegrationEventPublisher publisher,
    ILogger<OccasionSubmissionSentToReviewDomainEventHandler> logger)
    : IDomainEventHandler<OccasionSubmissionSentToReviewDomainEvent>
{
  
    public async Task HandleAsync(OccasionSubmissionSentToReviewDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            var moderationCase = ModerationCase.Create(
                ModerationCaseId.New(),
                domainEvent.OccasionSubmissionId);
            await repository.SaveAsync(moderationCase, cancellationToken);
            var integrationEvent = new SubmissionSentToReviewIntegrationEvent(domainEvent.OccasionSubmissionId, domainEvent.OccasionId);
            await publisher.PublishAsync(integrationEvent, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error handling OccasionSubmissionSentToReviewDomainEvent for SubmissionId: {SubmissionId}", domainEvent.OccasionSubmissionId);
            throw;  
        }
    }
}