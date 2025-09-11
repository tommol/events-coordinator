using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.Catalog.Commands;

public record CancelOccasionCommand(Guid OccasionId) : ICommand;