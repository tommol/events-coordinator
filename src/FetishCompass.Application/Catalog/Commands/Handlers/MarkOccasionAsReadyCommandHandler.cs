using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Catalog.Commands.Handlers;

public sealed class MarkOccasionAsReadyCommandHandler(
    IAggregateRepository<Occasion, OccasionId> occasionRepository,
    ILogger<MarkOccasionAsReadyCommandHandler> logger)
    : ICommandHandler<MarkOccasionAsReadyCommand>
{
    public async Task Handle(MarkOccasionAsReadyCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var occasion = await occasionRepository.GetByIdAsync((OccasionId)command.OccasionId, cancellationToken);
            if (occasion == null)
            {
                throw new InvalidOperationException($"Occasion with ID {command.OccasionId} not found.");
            }
            occasion.MarkAsReady();
            await occasionRepository.SaveAsync(occasion, cancellationToken);

        }catch (Exception e)
        {
            logger.LogError(e, "Error handling MarkOccasionAsReadyCommand for OccasionId: {OccasionId}", command.OccasionId);
            throw;  
        }
    }
}