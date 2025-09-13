using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.Catalog.Commands;

public sealed record CreateVenueCommand(
    string Name,
    string Country,
    string PostalCode,
    string City,
    string Street) : ICommand<Guid>;