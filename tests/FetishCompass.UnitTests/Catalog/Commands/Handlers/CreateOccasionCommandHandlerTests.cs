using FetishCompass.Application.Catalog.Commands;
using FetishCompass.Application.Catalog.Commands.Handlers;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FetishCompass.UnitTests.Catalog.Commands.Handlers
{
    public class CreateOccasionCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_SavesOccasionAndReturnsId()
        {
            var repo = Substitute.For<IAggregateRepository<Occasion, OccasionId>>();
            var logger = Substitute.For<ILogger<CreateOccasionCommandHandler>>();
            var handler = new CreateOccasionCommandHandler(repo, logger);
            
            var command = new CreateOccasionCommand(
                "Tytuł",
                "Opis",
                DateTime.Now,
                DateTime.Now.AddHours(1),
                "Europe/Warsaw",
                Guid.NewGuid(),
                Guid.NewGuid()
            );
            Occasion? savedOccasion = null;
            repo.SaveAsync(Arg.Do<Occasion>(o => savedOccasion = o), Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);

            var result = await handler.Handle(command);

            Assert.NotNull(savedOccasion);
            Assert.Equal(savedOccasion.Id, result);
            await repo.Received(1).SaveAsync(Arg.Any<Occasion>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Exception_LogsAndThrows()
        {
            var repo = Substitute.For<IAggregateRepository<Occasion, OccasionId>>();
            var logger = Substitute.For<ILogger<CreateOccasionCommandHandler>>();
            var handler = new CreateOccasionCommandHandler(repo, logger);
            var command = new CreateOccasionCommand(
                "Tytuł",
                "Opis",
                DateTime.Now,
                DateTime.Now.AddHours(1),
                "Europe/Warsaw",
                Guid.NewGuid(),
                Guid.NewGuid()
            );
            repo.SaveAsync(Arg.Any<Occasion>(), Arg.Any<CancellationToken>())
                .Returns<Task>(x => throw new Exception("db error"));

            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command));
        }
    }
}
