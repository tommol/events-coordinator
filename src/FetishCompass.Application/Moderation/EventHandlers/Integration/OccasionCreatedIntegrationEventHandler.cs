using FetishCompass.Application.Catalog.IntegrationEvents;
using FetishCompass.Application.Moderation.Commands;
using FetishCompass.Domain;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.IAM;
using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Application.Common;
using FetishCompass.Shared.Application.IntegrationEvents;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Moderation.EventHandlers.Integration;

public sealed class OccasionCreatedIntegrationEventHandler(
    IMediator mediator,
    ILogger<OccasionCreatedIntegrationEventHandler> logger)
    : IIntegrationEventHandler<OccasionCreatedIntegrationEvent>
{
    public async Task HandleAsync(OccasionCreatedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        try
        {
            var start = LocalDateTime.Create(
                DateOnly.FromDateTime(@event.LocalStart),
                TimeOnly.FromDateTime(@event.LocalStart),
                @event.TimeZone);
            var end = LocalDateTime.Create(
                DateOnly.FromDateTime(@event.LocalEnd),
                TimeOnly.FromDateTime(@event.LocalEnd),
                @event.TimeZone);
            var command = new CreateSubmissionCommand(
                OccasionSubmissionId.New(),
                (OccasionId)@event.OccasionId,
                @event.Title,
                @event.Description,
                start,
                end,
                (OrganizerAccountId)@event.Organizer,
                (VenueId)@event.Venue);
            await mediator.Send(command, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw;
        }
    }
}