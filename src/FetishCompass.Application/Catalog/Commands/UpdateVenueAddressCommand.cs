using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.Catalog.Commands;

public sealed record UpdateVenueAddressCommand(
    Guid VenueId,
    string Country,
    string PostalCode,
    string City,
    string Street) : ICommand;