using AutoFixture;
using FetishCompass.Application.Catalog.Commands;
using FetishCompass.Application.Catalog.EventHandlers.Integration;
using FetishCompass.Application.Catalog.IntegrationEvents;
using FetishCompass.Application.Moderation.IntegrationEvents;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Application.Common;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace FetishCompass.UnitTests.Catalog.Application.IntegrationEvents;

public sealed class SubmissionReviewedIntegrationEventHandlerTests
{
    [Fact]
    public async Task Handle_Approved_PublishesCommand()
    {
        var mediator = Substitute.For<IMediator>();
        var logger = Substitute.For<ILogger<OccasionSubmissionReviewedIntegrationEventHandler>>();
        var handler = new OccasionSubmissionReviewedIntegrationEventHandler(mediator, logger);
        var fixture = new Fixture();
        var receivedEvent = fixture.Build<OccasionSubmissionReviewedIntegrationEvent>()
            .With(x => x.Approved, true)
            .Create();

        await handler.HandleAsync(receivedEvent);

        await mediator.Received(1).Send(Arg.Any<PublishOccasionCommand>());
    }

    [Fact]
    public async Task Handle_Rejected_PublishesCommand()
    {
        var mediator = Substitute.For<IMediator>();
        var logger = Substitute.For<ILogger<OccasionSubmissionReviewedIntegrationEventHandler>>();
        var handler = new OccasionSubmissionReviewedIntegrationEventHandler(mediator, logger);
        var fixture = new Fixture();
        var receivedEvent = fixture.Build<OccasionSubmissionReviewedIntegrationEvent>()
            .With(x => x.Approved, false)
            .Create();

        await handler.HandleAsync(receivedEvent);

        await mediator.Received(1).Send(Arg.Any<DeleteOccasionCommand>());
    }
    
    [Fact]
    public async Task Handle_EncounterProblem_ThrowsAndLogs()
    {
        var mediator = Substitute.For<IMediator>();
        var logger = Substitute.For<ILogger<OccasionSubmissionReviewedIntegrationEventHandler>>();
        var handler = new OccasionSubmissionReviewedIntegrationEventHandler(mediator, logger);
        var fixture = new Fixture();
        var receivedEvent = fixture.Build<OccasionSubmissionReviewedIntegrationEvent>()
            .With(x => x.Approved, false)
            .Create();
        mediator.Send(Arg.Any<ICommand>()).Throws(new Exception());

        await Assert.ThrowsAnyAsync<Exception>(()=> handler.HandleAsync(receivedEvent));

        await mediator.Received(1).Send(Arg.Any<DeleteOccasionCommand>());
    }
}

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