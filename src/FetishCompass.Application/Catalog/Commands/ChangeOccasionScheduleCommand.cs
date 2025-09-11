using FetishCompass.Shared.Application.Commands;

namespace FetishCompass.Application.Catalog.Commands;

public record ChangeOccasionScheduleCommand(Guid OccasionId, DateTime LocalStart, DateTime LocalEnd, string Timezone) : ICommand;