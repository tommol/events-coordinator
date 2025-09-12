using FetishCompass.Application.Catalog.Commands;
using FetishCompass.Application.Moderation.IntegrationEvents;
using FetishCompass.Shared.Application.Common;
using FetishCompass.Shared.Application.IntegrationEvents;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Catalog.EventHandlers.Integration;

public sealed class SubmissionSentToReviewIntegrationEventHandler(
    IMediator mediator,
    ILogger<SubmissionSentToReviewIntegrationEventHandler> logger)
    : IIntegrationEventHandler<SubmissionSentToReviewIntegrationEvent>
{
    public async Task HandleAsync(SubmissionSentToReviewIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new MarkOccasionAsReadyCommand(@event.OccasionId);
            await mediator.Send(command, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw;
        }
    }
}