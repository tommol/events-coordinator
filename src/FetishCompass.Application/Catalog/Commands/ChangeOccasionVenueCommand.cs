using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.Catalog.Commands;

/// <summary>
/// Command used to change the venue of an occasion.
/// </summary>
/// <param name="OccasionId"></param>
/// <param name="Venue"></param>
public record ChangeOccasionVenueCommand(Guid OccasionId, Guid Venue) : ICommand;