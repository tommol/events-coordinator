using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.Catalog.Commands;

/// <summary>
/// Command used to update the details of an occasion.
/// </summary>
/// <param name="OccasionId"></param>
/// <param name="Title"></param>
/// <param name="Description"></param>
public record UpdateOccasionDetailCommand(Guid OccasionId, string Title, string Description) : ICommand;