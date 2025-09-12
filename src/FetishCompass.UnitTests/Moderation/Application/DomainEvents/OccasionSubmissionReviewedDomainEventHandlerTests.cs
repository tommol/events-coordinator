using FetishCompass.Application.Moderation.EventHandlers.Domain;
using FetishCompass.Application.Moderation.IntegrationEvents;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.Moderation.DomainEvents;
using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Application.IntegrationEvents;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FetishCompass.UnitTests.Moderation.Application.DomainEvents;

public class OccasionSubmissionReviewedDomainEventHandlerTests
{
    private readonly IIntegrationEventPublisher _publisher;
    private readonly ILogger<OccasionSubmissionReviewedDomainEventHandler> _logger;
    private readonly OccasionSubmissionReviewedDomainEventHandler _handler;

    public OccasionSubmissionReviewedDomainEventHandlerTests()
    {
        _publisher = Substitute.For<IIntegrationEventPublisher>();
        _logger = Substitute.For<ILogger<OccasionSubmissionReviewedDomainEventHandler>>();
        _handler = new OccasionSubmissionReviewedDomainEventHandler(_publisher, _logger);
    }

    [Fact]
    public async Task HandleAsync_ShouldPublishIntegrationEvent_WhenCalled()
    {
        // Arrange
        var domainEvent = new OccasionSubmissionReviewedDomainEvent(
            OccasionSubmissionId.New(), 
            (OccasionId)Guid.NewGuid(),
            SubmissionStatus.Accepted,
            DateTime.UtcNow);

        // Act
        await _handler.HandleAsync(domainEvent);

        // Assert
        await _publisher.Received(1).PublishAsync(
            Arg.Any<OccasionSubmissionReviewedIntegrationEvent>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldLogError_WhenExceptionIsThrown()
    {
        // Arrange
        var domainEvent = new OccasionSubmissionReviewedDomainEvent(
            OccasionSubmissionId.New(), 
            (OccasionId)Guid.NewGuid(),
            SubmissionStatus.Accepted,
            DateTime.UtcNow);

        _publisher
            .When(p => p.PublishAsync(Arg.Any<IIntegrationEvent>(), Arg.Any<CancellationToken>()))
            .Do(x => throw new Exception("Test exception"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.HandleAsync(domainEvent));
    }
}