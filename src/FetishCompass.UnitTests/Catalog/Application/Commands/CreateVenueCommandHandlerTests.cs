using FetishCompass.Application.Catalog.Commands;
using FetishCompass.Application.Catalog.Commands.Handlers;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FetishCompass.UnitTests.Catalog.Application.Commands;

public class CreateVenueCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_SavesVenueAndReturnsId()
    {
        var repo = Substitute.For<IAggregateRepository<Venue, VenueId>>();
        var logger = Substitute.For<ILogger<CreateVenueCommandHandler>>();
        var handler = new CreateVenueCommandHandler(repo, logger);

        var command = new CreateVenueCommand(
            "Tytuł",
            "Country",
            "00000",
            "City",
            "Street 1"
        );
        Venue? venue = null;
        repo.SaveAsync(Arg.Do<Venue>(o => venue = o), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        var result = await handler.Handle(command);

        Assert.NotNull(venue);
        Assert.Equal(venue.Id, result);
        await repo.Received(1).SaveAsync(Arg.Any<Venue>(), Arg.Any<CancellationToken>());
    }
    [Fact]
    public async Task Handle_Exception_LogsAndThrows()
    {
        var repo = Substitute.For<IAggregateRepository<Venue, VenueId>>();
        var logger = Substitute.For<ILogger<CreateVenueCommandHandler>>();
        var handler = new CreateVenueCommandHandler(repo, logger);

        var command = new CreateVenueCommand(
            "Tytuł",
            "Country",
            "00000",
            "City",
            "Street 1"
        );
        repo.SaveAsync(Arg.Any<Venue>(), Arg.Any<CancellationToken>())
            .Returns<Task>(x => throw new Exception("db error"));

        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command));
    }
}