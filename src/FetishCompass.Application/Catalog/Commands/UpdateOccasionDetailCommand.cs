using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.Catalog.Commands;

public record UpdateOccasionDetailCommand(Guid OccasionId, string Title, string Description) : ICommand;