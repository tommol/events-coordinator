using FetishCompass.Domain.Moderation.DomainEvents;
using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Domain;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Moderation.EventHandlers.Domain;

public sealed class OccasionSubmissionSentToReviewDomainEventHandler(
    IAggregateRepository<ModerationCase, ModerationCaseId> repository,
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
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error handling OccasionSubmissionSentToReviewDomainEvent for SubmissionId: {SubmissionId}", domainEvent.OccasionSubmissionId);
            throw;  
        }
    }
}