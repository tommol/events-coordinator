using AutoFixture;
using FetishCompass.Application.Catalog.Commands;
using FetishCompass.Application.Catalog.EventHandlers.Integration;
using FetishCompass.Application.Moderation.IntegrationEvents;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Application.Common;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace FetishCompass.UnitTests.Catalog.Application.IntegrationEvents;

public sealed class SubmissionSentToReviewIntegrationEventHandlerTests
{
    [Fact]
    public async Task Handle_PublishesCommand()
    {
        var mediator = Substitute.For<IMediator>();
        var logger = Substitute.For<ILogger<SubmissionSentToReviewIntegrationEventHandler>>();
        var handler = new SubmissionSentToReviewIntegrationEventHandler(mediator, logger);
        var fixture = new Fixture();
        var receivedEvent = fixture.Create<SubmissionSentToReviewIntegrationEvent>();
        
        await handler.HandleAsync(receivedEvent);

        await mediator.Received(1).Send(Arg.Any<MarkOccasionAsReadyCommand>());
    }

    
    [Fact]
    public async Task Handle_EncounterProblem_ThrowsAndLogs()
    {
        var mediator = Substitute.For<IMediator>();
        var logger = Substitute.For<ILogger<SubmissionSentToReviewIntegrationEventHandler>>();
        var handler = new SubmissionSentToReviewIntegrationEventHandler(mediator, logger);
        var fixture = new Fixture();
        var receivedEvent = fixture.Create<SubmissionSentToReviewIntegrationEvent>();
 
        mediator.Send(Arg.Any<ICommand>()).Throws(new Exception());

        await Assert.ThrowsAnyAsync<Exception>(()=> handler.HandleAsync(receivedEvent));

        await mediator.Received(1).Send(Arg.Any<MarkOccasionAsReadyCommand>());
    }
}