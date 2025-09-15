using AutoFixture;
using FetishCompass.Application.Catalog.Commands;
using FetishCompass.Application.Catalog.Commands.Handlers;
using FetishCompass.Application.IAM.Commands;
using FetishCompass.Application.IAM.Commands.Handlers;
using FetishCompass.Domain;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.IAM.Model;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FetishCompass.UnitTests.IAM.Application.Commands;

public sealed class CreateOrganizerAccountCommandHandlerTests
{
    [Theory]
    [InlineData(true, true, false)]
    [InlineData(true, false, true)]
    [InlineData(true, false, false)]
    [InlineData(false,true,false)]
    [InlineData(false,false,true)]
    [InlineData(false,false,false)]
    public async Task Handle_ValidCommand_SavesVenueAndReturnsId(bool needAccount, bool isPerson, bool isNonProfit)
    {
        var repo = Substitute.For<IAggregateRepository<OrganizerAccount, OrganizerAccountId>>();
        var logger = Substitute.For<ILogger<CreateOrganizerAccountCommandHandler>>();
        var handler = new CreateOrganizerAccountCommandHandler(repo, logger);

        var command = new CreateOrganizerAccountCommand(
            "Tytuł",
            "Country",
            needAccount,
            isPerson,
            isNonProfit);
        
        OrganizerAccount? venue = null;
        repo.SaveAsync(Arg.Do<OrganizerAccount>(o => venue = o), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        var result = await handler.Handle(command);

        Assert.NotNull(venue);
        Assert.Equal(venue.Id, result);
        await repo.Received(1).SaveAsync(Arg.Any<OrganizerAccount>(), Arg.Any<CancellationToken>());
    }
    [Fact]
    public async Task Handle_Exception_LogsAndThrows()
    {
        var repo = Substitute.For<IAggregateRepository<OrganizerAccount, OrganizerAccountId>>();
        var logger = Substitute.For<ILogger<CreateOrganizerAccountCommandHandler>>();
        var handler = new CreateOrganizerAccountCommandHandler(repo, logger);

        var command = new CreateOrganizerAccountCommand(
            "Tytuł",
            "Country",
            true,
            true,
            false);

        repo.SaveAsync(Arg.Any<OrganizerAccount>(), Arg.Any<CancellationToken>())
            .Returns<Task>(x => throw new Exception("db error"));

        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command));
    }
}

public class LinkAuthWithOrganizerCommandHandlerTests
{
    private readonly IFixture fixture = new Fixture();
    [Fact]
    public async Task Handle_ValidCommand_SavesVenueAndReturnsId()
    {
        var repo = Substitute.For<IAggregateRepository<OrganizerAccount, OrganizerAccountId>>();
        var logger = Substitute.For<ILogger<LinkAuthWithOrganizerCommandHandler>>();
        var handler = new LinkAuthWithOrganizerCommandHandler(repo, logger);
        var accountId = OrganizerAccountId.New();
        var command = new LinkAuthWithOrganizerCommand(
            accountId.Value,
            @"authId"
        );
        var name = fixture.Create<string>();
        var email = fixture.Create<string>();
        var account = OrganizerAccount.CreateCompanyWithLogin(accountId, name, email);
        repo.GetByIdAsync(accountId, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(account));

        await handler.Handle(command);
        
        await repo.Received(1).SaveAsync(Arg.Any<OrganizerAccount>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Handle_Exception_LogsAndThrows()
    {
        var repo = Substitute.For<IAggregateRepository<OrganizerAccount, OrganizerAccountId>>();
        var logger = Substitute.For<ILogger<LinkAuthWithOrganizerCommandHandler>>();
        var handler = new LinkAuthWithOrganizerCommandHandler(repo, logger);
        var accountId = OrganizerAccountId.New();
        var command = new LinkAuthWithOrganizerCommand(
            accountId.Value,
            @"authId"
        );
        var name = fixture.Create<string>();
        var email = fixture.Create<string>();
        var account = OrganizerAccount.CreateCompanyWithLogin(accountId, name, email);
        repo.GetByIdAsync(accountId, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(account));
        repo.SaveAsync(Arg.Any<OrganizerAccount>(), Arg.Any<CancellationToken>())
            .Returns<Task>(x => throw new Exception("db error"));

        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command));
    }
    
    [Fact]
    public async Task Handle_NotFound_LogsAndThrows()
    {
        var repo = Substitute.For<IAggregateRepository<OrganizerAccount, OrganizerAccountId>>();
        var logger = Substitute.For<ILogger<LinkAuthWithOrganizerCommandHandler>>();
        var handler = new LinkAuthWithOrganizerCommandHandler(repo, logger);
        var accountId = OrganizerAccountId.New();
        var command = new LinkAuthWithOrganizerCommand(
            accountId.Value,
            @"authId"
        );
       
        repo.GetByIdAsync(accountId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult((OrganizerAccount?)null));
        
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command));
    }
}