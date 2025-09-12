using AutoFixture;
using FetishCompass.Application.Catalog.Commands;
using FetishCompass.Application.Catalog.Commands.Handlers;
using FetishCompass.Domain;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.IAM;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FetishCompass.UnitTests.Catalog.Application.Commands;

public class DeleteOccasionCommandHandlerTests
{
    [Fact]
    public async Task Handle_OccasionExists_ChangesStatus()
    {
        var fixture = new Fixture();
        var repo = Substitute.For<IAggregateRepository<Occasion, OccasionId>>();
        var logger = Substitute.For<ILogger<DeleteOccasionCommandHandler>>();
        var handler = new DeleteOccasionCommandHandler(repo, logger);
        var start = fixture.Create<DateTime>();
        var id = Guid.NewGuid();
        var occasion = Occasion.Create(
            (OccasionId)id,
            "Tytuł test",
            "Opis test",
            OccasionSchedule.Create(
                LocalDateTime.Create(DateOnly.FromDateTime(start), TimeOnly.MinValue, "Europe/Warsaw"),
                LocalDateTime.Create(DateOnly.FromDateTime(start), TimeOnly.MaxValue, "Europe/Warsaw")),
            OccasionStatus.ReadyForReview,
            (OrganizerAccountId)Guid.NewGuid(),
            (VenueId)Guid.NewGuid()
        );
        repo.GetByIdAsync((OccasionId)id, Arg.Any<CancellationToken>()).Returns(occasion);
        var command = new DeleteOccasionCommand(occasion.Id);

        await handler.Handle(command);
            
        await repo.Received(1).SaveAsync(occasion, Arg.Any<CancellationToken>());
        Assert.Equal(OccasionStatus.Deleted,occasion.Status);
    }

    [Fact]
    public async Task Handle_OccasionNotFound_ThrowsAndLogs()
    {
        var repo = Substitute.For<IAggregateRepository<Occasion, OccasionId>>();
        var logger = Substitute.For<ILogger<DeleteOccasionCommandHandler>>();
        var handler = new DeleteOccasionCommandHandler(repo, logger);
        var id = Guid.NewGuid();
        repo.GetByIdAsync((OccasionId)id, Arg.Any<CancellationToken>()).Returns((Occasion?)null);
        var command = new DeleteOccasionCommand((OccasionId)id);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_Exception_LogsAndThrows()
    {
        var fixture = new Fixture();
        var repo = Substitute.For<IAggregateRepository<Occasion, OccasionId>>();
        var logger = Substitute.For<ILogger<DeleteOccasionCommandHandler>>();
        var handler = new DeleteOccasionCommandHandler(repo, logger);
        var start = fixture.Create<DateTime>();
        var id = Guid.NewGuid();
        var occasion = Occasion.Create(
            (OccasionId)id,
            "Tytuł test",
            "Opis test",
            OccasionSchedule.Create(
                LocalDateTime.Create(DateOnly.FromDateTime(start), TimeOnly.MinValue, "Europe/Warsaw"),
                LocalDateTime.Create(DateOnly.FromDateTime(start), TimeOnly.MaxValue, "Europe/Warsaw")),
            OccasionStatus.Published,
            (OrganizerAccountId)Guid.NewGuid(),
            (VenueId)Guid.NewGuid()
        );
        repo.GetByIdAsync((OccasionId)id, Arg.Any<CancellationToken>()).Returns(occasion);
        var command = new DeleteOccasionCommand(occasion.Id);
        repo.SaveAsync(occasion, Arg.Any<CancellationToken>()).Returns<Task>(x => throw new Exception("db error"));

        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command));
    }
}