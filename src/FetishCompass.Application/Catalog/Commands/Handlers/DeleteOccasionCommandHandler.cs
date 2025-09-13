using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Catalog.Commands.Handlers;

public sealed class DeleteOccasionCommandHandler(
    IAggregateRepository<Occasion, OccasionId> repository,
    ILogger<DeleteOccasionCommandHandler> logger)
    : ICommandHandler<DeleteOccasionCommand>
{
    public async Task Handle(DeleteOccasionCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var occasion = await repository.GetByIdAsync((OccasionId)command.OccasionId, cancellationToken);
            if (occasion == null)
            {
                throw new InvalidOperationException($"Occasion with ID {command.OccasionId} not found.");
            }
            occasion.Delete();
            await repository.SaveAsync(occasion, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }
}