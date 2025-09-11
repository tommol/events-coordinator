using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Catalog.Commands.Handlers;

public class CancelOccasionCommandHandler(IAggregateRepository<Occasion, OccasionId> repository, ILogger<CancelOccasionCommandHandler> logger)
    : ICommandHandler<CancelOccasionCommand>
{
    public async Task Handle(CancelOccasionCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var id = (OccasionId)command.OccasionId;
            var occasion = await repository.GetByIdAsync(id, cancellationToken);
            if (occasion == null)
            {
                logger.LogWarning("Occasion {OccasionId} not found", command.OccasionId);
                throw new InvalidOperationException($"Occasion {command.OccasionId} not found.");
            }

            occasion.Cancel();
            await repository.SaveAsync(occasion, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error cancelling occasion {OccasionId}", command.OccasionId);
            throw;
        }
    }
}
