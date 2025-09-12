using FetishCompass.Application.Moderation.Commands;
using FetishCompass.Application.Moderation.Commands.Handlers;
using FetishCompass.Domain;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.IAM;
using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FetishCompass.UnitTests.Moderation.Application.Commands;

public class RejectSubmissionCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_RejectsSubmission()
    {
        var repo = Substitute.For<IAggregateRepository<OccasionSubmission, OccasionSubmissionId>>();
        var logger = Substitute.For<ILogger<RejectSubmissionCommandHandler>>();
        var handler = new RejectSubmissionCommandHandler(repo, logger);
        var submission = OccasionSubmission.Create(
            OccasionSubmissionId.New(),
            "Title",
            "Description",
            LocalDateTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), TimeOnly.MinValue, "Europe/Warsaw"),
            LocalDateTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), TimeOnly.MaxValue, "Europe/Warsaw"),
            (OrganizerAccountId) Guid.NewGuid(),
            OccasionId.New(),
            VenueId.New());
        submission.SendToReview();
        var command = new RejectSubmissionCommand(new OccasionSubmissionId(Guid.NewGuid()), DateTimeOffset.UtcNow);
        repo.GetByIdAsync(command.SubmissionId, Arg.Any<CancellationToken>()).Returns(submission);

        await handler.Handle(command);
        
        await repo.Received(1).SaveAsync(submission, Arg.Any<CancellationToken>());
        Assert.Equal(SubmissionStatus.Rejected, submission.Status);
    }

    [Fact]
    public async Task Handle_SubmissionNotFound_Throws()
    {
        var repo = Substitute.For<IAggregateRepository<OccasionSubmission, OccasionSubmissionId>>();
        var logger = Substitute.For<ILogger<RejectSubmissionCommandHandler>>();
        var handler = new RejectSubmissionCommandHandler(repo, logger);
        var command = new RejectSubmissionCommand(new OccasionSubmissionId(Guid.NewGuid()), DateTimeOffset.UtcNow);
        repo.GetByIdAsync(command.SubmissionId, Arg.Any<CancellationToken>()).Returns((OccasionSubmission?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_Exception_LogsAndThrows()
    {
        var repo = Substitute.For<IAggregateRepository<OccasionSubmission, OccasionSubmissionId>>();
        var logger = Substitute.For<ILogger<RejectSubmissionCommandHandler>>();
        var handler = new RejectSubmissionCommandHandler(repo, logger);
        var submission = OccasionSubmission.Create(
            OccasionSubmissionId.New(),
            "Title",
            "Description",
            LocalDateTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), TimeOnly.MinValue, "Europe/Warsaw"),
            LocalDateTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), TimeOnly.MaxValue, "Europe/Warsaw"),
            (OrganizerAccountId) Guid.NewGuid(),
            OccasionId.New(),
            VenueId.New());
        var command = new RejectSubmissionCommand(new OccasionSubmissionId(Guid.NewGuid()), DateTimeOffset.UtcNow);
        repo.GetByIdAsync(command.SubmissionId, Arg.Any<CancellationToken>()).Returns(submission);
       
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command));
    }
}