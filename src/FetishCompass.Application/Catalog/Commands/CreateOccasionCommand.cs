using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.Catalog.Commands;

public record CreateOccasionCommand(string Title, string Description, DateTime LocalStart, DateTime LocalEnd, string Timezone, Guid Organizer, Guid Venue) : ICommand<Guid>;