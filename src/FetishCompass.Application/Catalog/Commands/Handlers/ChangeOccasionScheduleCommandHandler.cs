using FetishCompass.Domain;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Catalog.Commands.Handlers;

public class ChangeOccasionScheduleCommandHandler(AggregateRepository<Occasion, OccasionId> repository, ILogger<ChangeOccasionScheduleCommandHandler> logger)
    : ICommandHandler<ChangeOccasionScheduleCommand>
{
    public async Task Handle(ChangeOccasionScheduleCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var start = LocalDateTime.Create(DateOnly.FromDateTime(command.LocalStart), TimeOnly.FromDateTime(command.LocalStart), command.Timezone);
            var end = LocalDateTime.Create(DateOnly.FromDateTime(command.LocalEnd), TimeOnly.FromDateTime(command.LocalEnd), command.Timezone);

            var id = (OccasionId)command.OccasionId;
            var occasion = await repository.GetByIdAsync(id, cancellationToken);
            if (occasion == null)
            {
                logger.LogWarning("Occasion {OccasionId} not found", command.OccasionId);
                throw new InvalidOperationException($"Occasion {command.OccasionId} not found.");
            }

            occasion.UpdateSchedule(start, end);
            await repository.SaveAsync(occasion, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error changing occasion schedule {OccasionId}", command.OccasionId);
            throw;
        }
    }
}

