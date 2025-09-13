using FetishCompass.Domain;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Catalog.Commands.Handlers;

public sealed class UpdateVenueAddressCommandHandler(
    IAggregateRepository<Venue, VenueId> repository,
    ILogger<UpdateVenueAddressCommandHandler> logger)
    : ICommandHandler<UpdateVenueAddressCommand>
{
    public async Task Handle(UpdateVenueAddressCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var venue = await repository.GetByIdAsync((VenueId)command.VenueId, cancellationToken);
            if (venue == null)
            {
                throw new InvalidOperationException("Venue not found");
            }
            venue.UpdateAddress(Address.Create(
                command.Country,
                command.PostalCode,
                command.City,
                command.Street));
            await repository.SaveAsync(venue, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }
}