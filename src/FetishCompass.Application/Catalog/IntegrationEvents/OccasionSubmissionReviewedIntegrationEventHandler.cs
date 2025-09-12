using FetishCompass.Application.Catalog.Commands;
using FetishCompass.Application.Moderation.IntegrationEvents;
using FetishCompass.Shared.Application.Common;
using FetishCompass.Shared.Application.IntegrationEvents;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Catalog.IntegrationEvents;

public sealed class OccasionSubmissionReviewedIntegrationEventHandler(
    IMediator mediator,
    ILogger<OccasionSubmissionReviewedIntegrationEventHandler> logger)
    : IIntegrationEventHandler<OccasionSubmissionReviewedIntegrationEvent>
{
    public async Task HandleAsync(OccasionSubmissionReviewedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        try
        {
            if (@event.Approved)
            {
                var command = new PublishOccasionCommand(@event.OccasionId);
                await mediator.Send(command, cancellationToken);
            }
            else
            {
                var command = new DeleteOccasionCommand(@event.OccasionId);
                await mediator.Send(command, cancellationToken);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}