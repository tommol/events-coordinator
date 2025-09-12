using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.Catalog.Commands;

/// <summary>
/// Command to create a new Occasion in draft state.
/// </summary>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="LocalStart"></param>
/// <param name="LocalEnd"></param>
/// <param name="Timezone"></param>
/// <param name="Organizer"></param>
/// <param name="Venue"></param>
public record CreateOccasionCommand(string Title, string Description, DateTime LocalStart, DateTime LocalEnd, string Timezone, Guid Organizer, Guid Venue) : ICommand<Guid>;