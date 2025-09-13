using FetishCompass.Domain;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.IAM;
using FetishCompass.Domain.IAM.Model;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Catalog.Commands.Handlers;

public class CreateOccasionCommandHandler(IAggregateRepository<Occasion, OccasionId> repository, ILogger<CreateOccasionCommandHandler> logger)
    : ICommandHandler<CreateOccasionCommand, Guid>
{
    public async Task<Guid> Handle(CreateOccasionCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var start = LocalDateTime.Create(DateOnly.FromDateTime(command.LocalStart), TimeOnly.FromDateTime(command.LocalStart), command.Timezone);
            var end = LocalDateTime.Create(DateOnly.FromDateTime(command.LocalEnd), TimeOnly.FromDateTime(command.LocalEnd), command.Timezone);

            var occasion = Occasion.Create(
                 OccasionId.New(),
                command.Title,
                command.Description,
                OccasionSchedule.Create(start, end),
                    OccasionStatus.Draft,
                (OrganizerAccountId)command.Organizer,
                (VenueId)command.Venue);
            
            await repository.SaveAsync(occasion, cancellationToken);
            return occasion.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating occasion");
            throw;
        }
    }
}