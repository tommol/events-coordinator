using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace FetishCompass.Application.Moderation.Commands.Handlers;

public sealed class AcceptSubmissionCommandHandler(
    IAggregateRepository<OccasionSubmission, OccasionSubmissionId> repository,
    ILogger<AcceptSubmissionCommandHandler> logger)
    : ICommandHandler<AcceptSubmissionCommand>
{
    public async Task Handle(AcceptSubmissionCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var submission = await repository.GetByIdAsync(command.SubmissionId, cancellationToken);
            if (submission is null)
                throw new InvalidOperationException($"Submission with id {command.SubmissionId} not found");
            submission.Accept(command.ReviewedAt);
            await repository.SaveAsync(submission, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error handling AcceptSubmissionCommand for SubmissionId: {SubmissionId}", command.SubmissionId);
            throw;
        }
    }
}