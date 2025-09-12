using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.Catalog.Commands;

/// <summary>
/// Command used to cancel an occasion.
/// </summary>
/// <param name="OccasionId"></param>
public record CancelOccasionCommand(Guid OccasionId) : ICommand;