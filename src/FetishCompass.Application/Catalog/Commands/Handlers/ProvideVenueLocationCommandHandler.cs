using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Catalog.Commands.Handlers;

public sealed class ProvideVenueLocationCommandHandler(
    IAggregateRepository<Venue, VenueId> repository,
    ILogger<ProvideVenueLocationCommandHandler> logger) : ICommandHandler<ProvideVenueLocationCommand>
{
    public async Task Handle(ProvideVenueLocationCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var venue = await repository.GetByIdAsync((VenueId)command.VenueId, cancellationToken);
            if (venue == null)
            {
                throw new InvalidOperationException("Venue not found");
            }
            venue.GeoLocate(command.Latitude, command.Longitude);
            await repository.SaveAsync(venue, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }
}