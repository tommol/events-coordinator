using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Moderation.Commands.Handlers;

public sealed class CreateSubmissionCommandHandler(
    IAggregateRepository<OccasionSubmission, OccasionSubmissionId> repository,
    ILogger<CreateSubmissionCommandHandler> logger)
    : ICommandHandler<CreateSubmissionCommand>
{
    public async Task Handle(CreateSubmissionCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var submission = OccasionSubmission.Create(
                command.SubmissionId,
                command.Title,
                command.Description,
                command.Start,
                command.End,
                command.Organizer,
                command.OccasionId,
                command.Venue);

             await repository.SaveAsync(submission, cancellationToken);
        }catch (Exception e)
        {
            logger.LogError(e, "Error handling CreateSubmissionCommand for SubmissionId: {SubmissionId}", command.SubmissionId);
            throw;  
        }
    }
}