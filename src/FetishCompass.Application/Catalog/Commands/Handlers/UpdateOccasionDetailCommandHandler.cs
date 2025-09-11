using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Catalog.Commands.Handlers;

public class UpdateOccasionDetailCommandHandler(IAggregateRepository<Occasion, OccasionId> repository, ILogger<UpdateOccasionDetailCommandHandler> logger)
    : ICommandHandler<UpdateOccasionDetailCommand>
{
    public async Task Handle(UpdateOccasionDetailCommand command, CancellationToken cancellationToken = default)
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

            occasion.UpdateDetails(command.Title, command.Description);
            await repository.SaveAsync(occasion, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating occasion details {OccasionId}", command.OccasionId);
            throw;
        }
    }
}
