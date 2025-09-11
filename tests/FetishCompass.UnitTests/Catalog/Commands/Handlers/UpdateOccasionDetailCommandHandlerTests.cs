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
    public class UpdateOccasionDetailCommandHandlerTests
    {
        private IFixture fixture = new Fixture();

        [Fact]
        public async Task Handle_OccasionExists_UpdatesAndSaves()
        {
            var repo = Substitute.For<IAggregateRepository<Occasion, OccasionId>>();
            var logger = Substitute.For<ILogger<UpdateOccasionDetailCommandHandler>>();
            var handler = new UpdateOccasionDetailCommandHandler(repo, logger);
            var start = fixture.Create<DateTime>();
            var id = Guid.NewGuid();
            var occasion = Occasion.Create(
                (OccasionId)id,
                "Tytuł test",
                "Opis test",
                OccasionSchedule.Create(
                    LocalDateTime.Create(DateOnly.FromDateTime(start), TimeOnly.MinValue, "Europe/Warsaw"),
                    LocalDateTime.Create(DateOnly.FromDateTime(start), TimeOnly.MaxValue, "Europe/Warsaw")),
                OccasionStatus.Draft,
                (OrganizerAccountId)Guid.NewGuid(),
                (VenueId)Guid.NewGuid()
            );
            repo.GetByIdAsync((OccasionId)id, Arg.Any<CancellationToken>()).Returns(occasion);
            var command = new UpdateOccasionDetailCommand(id, "Nowy", "Opis");

            await handler.Handle(command);
            
            await repo.Received(1).SaveAsync(occasion, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_OccasionNotFound_ThrowsAndLogs()
        {
            var repo = Substitute.For<IAggregateRepository<Occasion, OccasionId>>();
            var logger = Substitute.For<ILogger<UpdateOccasionDetailCommandHandler>>();
            var handler = new UpdateOccasionDetailCommandHandler(repo, logger);
            var id = Guid.NewGuid();
            repo.GetByIdAsync((OccasionId)id, Arg.Any<CancellationToken>()).Returns((Occasion?)null);
            var command = new UpdateOccasionDetailCommand(id, "Nowy", "Opis");

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command));
        }

        [Fact]
        public async Task Handle_Exception_LogsAndThrows()
        {
            var repo = Substitute.For<IAggregateRepository<Occasion, OccasionId>>();
            var logger = Substitute.For<ILogger<UpdateOccasionDetailCommandHandler>>();
            var handler = new UpdateOccasionDetailCommandHandler(repo, logger);
            var start = fixture.Create<DateTime>();
            var id = Guid.NewGuid();
            var occasion = Occasion.Create(
                (OccasionId)id,
                "Tytuł test",
                "Opis test",
                OccasionSchedule.Create(
                    LocalDateTime.Create(DateOnly.FromDateTime(start), TimeOnly.MinValue, "Europe/Warsaw"),
                    LocalDateTime.Create(DateOnly.FromDateTime(start), TimeOnly.MaxValue, "Europe/Warsaw")),
                OccasionStatus.Draft,
                (OrganizerAccountId)Guid.NewGuid(),
                (VenueId)Guid.NewGuid()
            );
            repo.GetByIdAsync((OccasionId)id, Arg.Any<CancellationToken>()).Returns(occasion);
            var command = new UpdateOccasionDetailCommand(id, "Nowy", "Opis");
            repo.SaveAsync(occasion, Arg.Any<CancellationToken>()).Returns<Task>(x => throw new Exception("db error"));

            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command));
        }
    }
}
