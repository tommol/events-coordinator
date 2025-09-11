using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Catalog.Commands.Handlers;

public class ChangeOccasionVenueCommandHandler(IAggregateRepository<Occasion, OccasionId> repository, ILogger<ChangeOccasionVenueCommandHandler> logger)
    : ICommandHandler<ChangeOccasionVenueCommand>
{
    public async Task Handle(ChangeOccasionVenueCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var id = (OccasionId)command.OccasionId;
            var newVenue = (VenueId)command.Venue;

            var occasion = await repository.GetByIdAsync(id, cancellationToken);
            if (occasion == null)
            {
                logger.LogWarning("Occasion {OccasionId} not found", command.OccasionId);
                throw new InvalidOperationException($"Occasion {command.OccasionId} not found.");
            }

            occasion.ChangeVenue(newVenue);
            await repository.SaveAsync(occasion, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error changing occasion venue {OccasionId}", command.OccasionId);
            throw;
        }
    }
}
