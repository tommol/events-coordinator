using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.Catalog.Commands;

public record ChangeOccasionVenueCommand(Guid OccasionId, Guid Venue) : ICommand;