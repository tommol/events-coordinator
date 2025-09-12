using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.Catalog.Commands;

/// <summary>
/// Command used to change the schedule of an occasion.
/// </summary>
/// <param name="OccasionId"></param>
/// <param name="LocalStart"></param>
/// <param name="LocalEnd"></param>
/// <param name="Timezone"></param>
public record ChangeOccasionScheduleCommand(Guid OccasionId, DateTime LocalStart, DateTime LocalEnd, string Timezone) : ICommand;