using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FetishCompass.Application.Catalog.Commands;
using FetishCompass.Application.Catalog.Commands.Handlers;
using FetishCompass.Domain;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.IAM;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace FetishCompass.UnitTests.Catalog.Commands.Handlers
{
    public class CancelOccasionCommandHandlerTests
    {
        [Fact]
        public async Task Handle_OccasionExists_CancelsAndSaves()
        {
            var fixture = new Fixture();
            var repo = Substitute.For<IAggregateRepository<Occasion, OccasionId>>();
            var logger = Substitute.For<ILogger<CancelOccasionCommandHandler>>();
            var handler = new CancelOccasionCommandHandler(repo, logger);
            var id = Guid.NewGuid();
            var occasion = Occasion.Create(
                (OccasionId)id,
                "Tytuł test",
                "Opis test",
                OccasionSchedule.Create(
                    LocalDateTime.Create(DateOnly.FromDateTime(fixture.Create<DateTime>()), TimeOnly.MinValue, "Europe/Warsaw"),
                    LocalDateTime.Create(DateOnly.FromDateTime(fixture.Create<DateTime>()), TimeOnly.MaxValue, "Europe/Warsaw")),
                OccasionStatus.Draft,
                (OrganizerAccountId)Guid.NewGuid(),
                (VenueId)Guid.NewGuid()
            );
            repo.GetByIdAsync((OccasionId)id, Arg.Any<CancellationToken>()).Returns(occasion);
            var command = new CancelOccasionCommand(id);

            await handler.Handle(command);

            occasion.Received().Cancel();
            await repo.Received(1).SaveAsync(occasion, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_OccasionNotFound_ThrowsAndLogs()
        {
            var repo = Substitute.For<IAggregateRepository<Occasion, OccasionId>>();
            var logger = Substitute.For<ILogger<CancelOccasionCommandHandler>>();
            var handler = new CancelOccasionCommandHandler(repo, logger);
            var id = Guid.NewGuid();
            repo.GetByIdAsync((OccasionId)id, Arg.Any<CancellationToken>()).Returns((Occasion?)null);
            var command = new CancelOccasionCommand(id);

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command));
            logger.Received().Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("not found")),
                null,
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact]
        public async Task Handle_Exception_LogsAndThrows()
        {
            var fixture = new Fixture();
            var repo = Substitute.For<IAggregateRepository<Occasion, OccasionId>>();
            var logger = Substitute.For<ILogger<CancelOccasionCommandHandler>>();
            var handler = new CancelOccasionCommandHandler(repo, logger);
            var id = Guid.NewGuid();
            var occasion = Occasion.Create(
                (OccasionId)id,
                "Tytuł test",
                "Opis test",
                OccasionSchedule.Create(
                    LocalDateTime.Create(DateOnly.FromDateTime(fixture.Create<DateTime>()), TimeOnly.MinValue, "Europe/Warsaw"),
                    LocalDateTime.Create(DateOnly.FromDateTime(fixture.Create<DateTime>()), TimeOnly.MaxValue, "Europe/Warsaw")),
                OccasionStatus.Draft,
                (OrganizerAccountId)Guid.NewGuid(),
                (VenueId)Guid.NewGuid()
            );
            repo.GetByIdAsync((OccasionId)id, Arg.Any<CancellationToken>()).Returns(occasion);
            var command = new CancelOccasionCommand(id);
            repo.SaveAsync(occasion, Arg.Any<CancellationToken>()).Returns<Task>(x => throw new Exception("db error"));

            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command));
            logger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains("Error cancelling occasion")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }
    }
}
