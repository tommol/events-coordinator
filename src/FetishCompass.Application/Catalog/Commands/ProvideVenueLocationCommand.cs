using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.Catalog.Commands;

public sealed record ProvideVenueLocationCommand(
    Guid VenueId,
    double Latitude,
    double Longitude) : ICommand;