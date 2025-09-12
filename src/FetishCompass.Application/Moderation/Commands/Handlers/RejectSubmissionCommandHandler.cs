using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Moderation.Commands.Handlers;

public sealed class RejectSubmissionCommandHandler(
    IAggregateRepository<OccasionSubmission, OccasionSubmissionId> repository,
    ILogger<RejectSubmissionCommandHandler> logger)
    : ICommandHandler<RejectSubmissionCommand>
{
    public async Task Handle(RejectSubmissionCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var submission = await repository.GetByIdAsync(command.SubmissionId, cancellationToken);
            if (submission is null)
                throw new InvalidOperationException($"Submission with id {command.SubmissionId} not found");
            submission.Reject(command.ReviewedAt);
            await repository.SaveAsync(submission, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error handling RejectSubmissionCommand for SubmissionId: {SubmissionId}", command.SubmissionId);
            throw;
        }
    }
}