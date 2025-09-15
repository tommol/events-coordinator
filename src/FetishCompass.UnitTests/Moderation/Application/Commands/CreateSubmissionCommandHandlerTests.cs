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
    public class CreateSubmissionCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_CreatesSubmission()
        {
            var repo = Substitute.For<IAggregateRepository<OccasionSubmission, OccasionSubmissionId>>();
            var logger = Substitute.For<ILogger<CreateSubmissionCommandHandler>>();
            var handler = new CreateSubmissionCommandHandler(repo, logger);
            var submissionId = OccasionSubmissionId.New();
            var occasionId = OccasionId.New();
            var title = "Test title";
            var description = "Test description";
            var start = LocalDateTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), TimeOnly.MinValue, "Europe/Warsaw");
            var end = LocalDateTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), TimeOnly.MaxValue, "Europe/Warsaw");
            var organizer = (OrganizerAccountId)Guid.NewGuid();
            var venue = VenueId.New();
            var command = new CreateSubmissionCommand(submissionId, occasionId, title, description, start, end, organizer, venue);

            await handler.Handle(command);

            await repo.Received(1).SaveAsync(Arg.Is<OccasionSubmission>(s =>
                s.ProposedOccasion.Title == title &&
                s.ProposedOccasion.Description == description &&
                s.ProposedOccasion.StartDate == start &&
                s.ProposedOccasion.EndDate == end &&
                s.ProposedOccasion.Organizer == organizer &&
                s.ProposedOccasion.Venue == venue &&
                s.Status == SubmissionStatus.Received
            ), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Exception_LogsAndThrows()
        {
            var repo = Substitute.For<IAggregateRepository<OccasionSubmission, OccasionSubmissionId>>();
            var logger = Substitute.For<ILogger<CreateSubmissionCommandHandler>>();
            var handler = new CreateSubmissionCommandHandler(repo, logger);
            var submissionId = OccasionSubmissionId.New();
            var occasionId = OccasionId.New();
            var title = "Test title";
            var description = "Test description";
            var start = LocalDateTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), TimeOnly.MinValue, "Europe/Warsaw");
            var end = LocalDateTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), TimeOnly.MaxValue, "Europe/Warsaw");
            var organizer = (OrganizerAccountId)Guid.NewGuid();
            var venue = VenueId.New();
            var command = new CreateSubmissionCommand(submissionId, occasionId, title, description, start, end, organizer, venue);
            repo.When(x => x.SaveAsync(Arg.Any<OccasionSubmission>(), Arg.Any<CancellationToken>()))
                .Do(_ => throw new InvalidOperationException("fail"));

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command));
        }
    }
}
