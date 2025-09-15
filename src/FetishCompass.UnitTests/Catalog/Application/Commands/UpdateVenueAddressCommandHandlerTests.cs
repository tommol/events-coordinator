using AutoFixture;
using FetishCompass.Application.Catalog.Commands;
using FetishCompass.Application.Catalog.Commands.Handlers;
using FetishCompass.Domain;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FetishCompass.UnitTests.Catalog.Application.Commands;

public class UpdateVenueAddressCommandHandlerTests
{
    private readonly IFixture fixture = new Fixture();
    [Fact]
    public async Task Handle_ValidCommand_SavesVenueAndReturnsId()
    {
        var repo = Substitute.For<IAggregateRepository<Venue, VenueId>>();
        var logger = Substitute.For<ILogger< UpdateVenueAddressCommandHandler>>();
        var handler = new  UpdateVenueAddressCommandHandler(repo, logger);
        var venueId = VenueId.New();
        var command = new UpdateVenueAddressCommand(
            venueId.Value,
            "Country",
            "00000",
            "City",
            "Street 1"
        );
        var address = fixture.Create<Address>();
        Venue venue = Venue.Create(
            venueId,
            "name",
            address);
        repo.GetByIdAsync(venueId, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(venue));

        await handler.Handle(command);

        Assert.NotNull(venue);
        Assert.Equal(command.Country, venue.Address.Country);
        Assert.Equal(command.PostalCode, venue.Address.PostalCode);
        Assert.Equal(command.City, venue.Address.City);
        Assert.Equal(command.Street, venue.Address.Street);
        await repo.Received(1).SaveAsync(Arg.Any<Venue>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Handle_Exception_LogsAndThrows()
    {
        var repo = Substitute.For<IAggregateRepository<Venue, VenueId>>();
        var logger = Substitute.For<ILogger< UpdateVenueAddressCommandHandler>>();
        var handler = new  UpdateVenueAddressCommandHandler(repo, logger);
        var venueId = VenueId.New();
        var command = new UpdateVenueAddressCommand(
            venueId.Value,
            "Country",
            "00000",
            "City",
            "Street 1"
        );
        var address = fixture.Create<Address>();
        Venue venue = Venue.Create(
            venueId,
            "name",
            address);
        repo.GetByIdAsync(venueId, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(venue));

        await handler.Handle(command);
        repo.SaveAsync(Arg.Any<Venue>(), Arg.Any<CancellationToken>())
            .Returns<Task>(x => throw new Exception("db error"));

        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command));
    }
    
    [Fact]
    public async Task Handle_NotFound_LogsAndThrows()
    {
        var repo = Substitute.For<IAggregateRepository<Venue, VenueId>>();
        var logger = Substitute.For<ILogger< UpdateVenueAddressCommandHandler>>();
        var handler = new  UpdateVenueAddressCommandHandler(repo, logger);
        var venueId = VenueId.New();
        var command = new UpdateVenueAddressCommand(
            venueId.Value,
            "Country",
            "00000",
            "City",
            "Street 1"
        );
        var address = fixture.Create<Address>();
        Venue venue = Venue.Create(
            venueId,
            "name",
            address);
        repo.GetByIdAsync(venueId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult((Venue?)null));
    
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command));
    }
}