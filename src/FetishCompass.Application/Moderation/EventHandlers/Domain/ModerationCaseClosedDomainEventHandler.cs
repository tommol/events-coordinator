using FetishCompass.Domain.Moderation.DomainEvents;
using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Domain;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Moderation.EventHandlers.Domain;

public sealed class ModerationCaseClosedDomainEventHandler(
    IAggregateRepository<OccasionSubmission, OccasionSubmissionId> repository,
    ILogger<ModerationCaseClosedDomainEventHandler> logger) 
    : IDomainEventHandler<ModerationCaseClosedDomainEvent>
{
    public async Task HandleAsync(ModerationCaseClosedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            var occasionSubmission = await repository.GetByIdAsync(domainEvent.Submission, cancellationToken);
            if (occasionSubmission == null)
            {
                throw new InvalidOperationException("Occasion submission not found");
            }
            if (domainEvent.PositiveResolution)
            {
                occasionSubmission.Accept(domainEvent.ClosedAt);
            }
            else
            {
                occasionSubmission.Reject(domainEvent.ClosedAt);
            }
            await repository.SaveAsync(occasionSubmission, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error handling ModerationCaseClosedDomainEvent for ModerationCaseId: {ModerationCaseId}", domainEvent.ModerationCaseId);
            throw;  
        }
    }
}