using AutoFixture;
using FetishCompass.Application.Catalog.IntegrationEvents;
using FetishCompass.Application.Moderation.Commands;
using FetishCompass.Application.Moderation.EventHandlers.Integration;
using FetishCompass.Shared.Application.Commands;
using FetishCompass.Shared.Application.Common;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace FetishCompass.UnitTests.Moderation.Application.IntegrationEvents;

public sealed class OccasionCreatedIntegrationEventHandlerTests
{
    [Fact]
    public async Task Handle_PublishesCommand()
    {
        var mediator = Substitute.For<IMediator>();
        var logger = Substitute.For<ILogger<OccasionCreatedIntegrationEventHandler>>();
        var handler = new OccasionCreatedIntegrationEventHandler(mediator, logger);
        var fixture = new Fixture();
        var receivedEvent = fixture.Build<OccasionCreatedIntegrationEvent>()
            .With(x=>x.LocalStart, DateTime.Now)
            .With(x=>x.LocalEnd, DateTime.Now.AddHours(4))
            .With(x=>x.TimeZone,"Europe/Warsaw")
            .Create();
        
        await handler.HandleAsync(receivedEvent);

        await mediator.Received(1).Send(Arg.Any<CreateSubmissionCommand>());
    }

    
    [Fact]
    public async Task Handle_EncounterProblem_ThrowsAndLogs()
    {
        var mediator = Substitute.For<IMediator>();
        var logger = Substitute.For<ILogger<OccasionCreatedIntegrationEventHandler>>();
        var handler = new OccasionCreatedIntegrationEventHandler(mediator, logger);
        var fixture = new Fixture();
        var receivedEvent = fixture.Build<OccasionCreatedIntegrationEvent>()
            .With(x=>x.LocalStart, DateTime.Now)
            .With(x=>x.LocalEnd, DateTime.Now.AddHours(4))
            .With(x=>x.TimeZone,"Europe/Warsaw")
            .Create();
        mediator.Send(Arg.Any<ICommand>()).Throws(new Exception());

        await Assert.ThrowsAnyAsync<Exception>(()=> handler.HandleAsync(receivedEvent));

        await mediator.Received(1).Send(Arg.Any<CreateSubmissionCommand>());
    }
}