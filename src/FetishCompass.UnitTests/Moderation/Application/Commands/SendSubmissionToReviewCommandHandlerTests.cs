using FetishCompass.Application.Moderation.Commands;
using FetishCompass.Application.Moderation.Commands.Handlers;
using FetishCompass.Domain;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.IAM.Model;
using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FetishCompass.UnitTests.Moderation.Application.Commands
{
    public class SendSubmissionToReviewCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_SendsToReview()
        {
            var repo = Substitute.For<IAggregateRepository<OccasionSubmission, OccasionSubmissionId>>();
            var logger = Substitute.For<ILogger<SendSubmissionToReviewCommandHandler>>();
            var handler = new SendSubmissionToReviewCommandHandler(repo, logger);
            var submission = OccasionSubmission.Create(
                OccasionSubmissionId.New(),
                "Title",
                "Description",
                LocalDateTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), TimeOnly.MinValue, "Europe/Warsaw"),
                LocalDateTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), TimeOnly.MaxValue, "Europe/Warsaw"),
                (OrganizerAccountId) Guid.NewGuid(),
                OccasionId.New(),
                VenueId.New());
            var command = new SendSubmissionToReviewCommand(new OccasionSubmissionId(Guid.NewGuid()));
            repo.GetByIdAsync(command.SubmissionId, Arg.Any<CancellationToken>()).Returns(submission);

            await handler.Handle(command);
            
            await repo.Received(1).SaveAsync(submission, Arg.Any<CancellationToken>());
            Assert.Equal(SubmissionStatus.UnderReview, submission.Status);
        }

        [Fact]
        public async Task Handle_SubmissionNotFound_Throws()
        {
            var repo = Substitute.For<IAggregateRepository<OccasionSubmission, OccasionSubmissionId>>();
            var logger = Substitute.For<ILogger<SendSubmissionToReviewCommandHandler>>();
            var handler = new SendSubmissionToReviewCommandHandler(repo, logger);
            var command = new SendSubmissionToReviewCommand(new OccasionSubmissionId(Guid.NewGuid()));
            repo.GetByIdAsync(command.SubmissionId, Arg.Any<CancellationToken>()).Returns((OccasionSubmission?)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command));
        }

        [Fact]
        public async Task Handle_Exception_LogsAndThrows()
        {
            var repo = Substitute.For<IAggregateRepository<OccasionSubmission, OccasionSubmissionId>>();
            var logger = Substitute.For<ILogger<SendSubmissionToReviewCommandHandler>>();
            var handler = new SendSubmissionToReviewCommandHandler(repo, logger);
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
            submission.Accept(DateTimeOffset.Now);
            
            var command = new SendSubmissionToReviewCommand(new OccasionSubmissionId(Guid.NewGuid()));
            repo.GetByIdAsync(command.SubmissionId, Arg.Any<CancellationToken>()).Returns(submission);
            
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command));
        }
    }
}