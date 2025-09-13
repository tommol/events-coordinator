using FetishCompass.Application.Moderation.EventHandlers.Domain;
using FetishCompass.Application.Moderation.IntegrationEvents;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.Moderation.DomainEvents;
using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Application.IntegrationEvents;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FetishCompass.UnitTests.Moderation.Application.DomainEvents;

public class OccasionSubmissionSentToReviewDomainEventHandlerTests
{
    private readonly IAggregateRepository<ModerationCase, ModerationCaseId> repository;
    private readonly IIntegrationEventPublisher publisher;
    private readonly ILogger<OccasionSubmissionSentToReviewDomainEventHandler> logger;
    private readonly OccasionSubmissionSentToReviewDomainEventHandler handler;

    public OccasionSubmissionSentToReviewDomainEventHandlerTests()
    {
        repository = Substitute.For<IAggregateRepository<ModerationCase, ModerationCaseId>>();
        publisher = Substitute.For<IIntegrationEventPublisher>();
        logger = Substitute.For<ILogger<OccasionSubmissionSentToReviewDomainEventHandler>>();
        handler = new OccasionSubmissionSentToReviewDomainEventHandler(repository,publisher, logger);
    }

    [Fact]
    public async Task HandleAsync_ShouldPublishIntegrationEvent_WhenCalled()
    {
        // Arrange
        var domainEvent = new OccasionSubmissionSentToReviewDomainEvent(
            OccasionSubmissionId.New(), 
            (OccasionId)Guid.NewGuid());

        // Act
        await handler.HandleAsync(domainEvent);

        // Assert
        await publisher.Received(1).PublishAsync(
            Arg.Any<SubmissionSentToReviewIntegrationEvent>(),
            Arg.Any<CancellationToken>());
        await repository.Received(1).SaveAsync(
            Arg.Any<ModerationCase>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldLogError_WhenExceptionIsThrown()
    {
        // Arrange
        var domainEvent = new OccasionSubmissionSentToReviewDomainEvent(
            OccasionSubmissionId.New(), 
            (OccasionId)Guid.NewGuid());

        publisher
            .When(p => p.PublishAsync(Arg.Any<IIntegrationEvent>(), Arg.Any<CancellationToken>()))
            .Do(x => throw new Exception("Test exception"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => handler.HandleAsync(domainEvent));
    }
}