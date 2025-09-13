using FetishCompass.Domain;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Catalog.Commands.Handlers;

public sealed class CreateVenueCommandHandler(
    IAggregateRepository<Venue, VenueId> repository,
    ILogger<CreateVenueCommandHandler> logger)
    : ICommandHandler<CreateVenueCommand, Guid>
{
    public async Task<Guid> Handle(CreateVenueCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var venue = Venue.Create(
                VenueId.New(),
                command.Name,
                Address.Create(
                    command.Country,
                    command.PostalCode,
                    command.City,
                    command.Street));
            await repository.SaveAsync(venue, cancellationToken);
            return venue.Id;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }
}