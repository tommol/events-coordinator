using FetishCompass.Application.Moderation.EventHandlers.Domain;
using FetishCompass.Domain;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.IAM.Model;
using FetishCompass.Domain.Moderation.DomainEvents;
using FetishCompass.Domain.Moderation.Model;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FetishCompass.UnitTests.Moderation.Application.DomainEvents;

public class ModerationCaseClosedDomainEventHandlerTests
{
    private readonly IAggregateRepository<OccasionSubmission, OccasionSubmissionId> _repository;
    private readonly ILogger<ModerationCaseClosedDomainEventHandler> _logger;
    private readonly ModerationCaseClosedDomainEventHandler _handler;

    public ModerationCaseClosedDomainEventHandlerTests()
    {
        _repository = Substitute.For<IAggregateRepository<OccasionSubmission, OccasionSubmissionId>>();
        _logger = Substitute.For<ILogger<ModerationCaseClosedDomainEventHandler>>();
        _handler = new ModerationCaseClosedDomainEventHandler(_repository, _logger);
    }

    [Fact]
    public async Task HandleAsync_ShouldAcceptSubmission_WhenPositiveResolution()
    {
        // Arrange
        var domainEvent = new ModerationCaseClosedDomainEvent(
            ModerationCaseId.New(),
            new OccasionSubmissionId(Guid.NewGuid()),
            DateTime.UtcNow,
            true,
            null);

        var occasionSubmission  = OccasionSubmission.Create(
            OccasionSubmissionId.New(),
            "Title",
            "Description",
            LocalDateTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), TimeOnly.MinValue, "Europe/Warsaw"),
            LocalDateTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), TimeOnly.MaxValue, "Europe/Warsaw"),
            (OrganizerAccountId) Guid.NewGuid(),
            OccasionId.New(),
            VenueId.New());
        occasionSubmission.SendToReview();
        _repository.GetByIdAsync(domainEvent.Submission, Arg.Any<CancellationToken>())
            .Returns(occasionSubmission);

        // Act
        await _handler.HandleAsync(domainEvent);

        // Assert
        await _repository.Received(1).SaveAsync(occasionSubmission, Arg.Any<CancellationToken>());
        Assert.Equal(SubmissionStatus.Accepted, occasionSubmission.Status);
    }

    [Fact]
    public async Task HandleAsync_ShouldRejectSubmission_WhenNegativeResolution()
    {
        // Arrange
        var domainEvent = new ModerationCaseClosedDomainEvent(
            ModerationCaseId.New(),
            new OccasionSubmissionId(Guid.NewGuid()),
            DateTime.UtcNow,
            false,
            null);

        var occasionSubmission = OccasionSubmission.Create(
            OccasionSubmissionId.New(),
            "Title",
            "Description",
            LocalDateTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), TimeOnly.MinValue, "Europe/Warsaw"),
            LocalDateTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), TimeOnly.MaxValue, "Europe/Warsaw"),
            (OrganizerAccountId) Guid.NewGuid(),
            OccasionId.New(),
            VenueId.New());
        occasionSubmission.SendToReview();
        _repository.GetByIdAsync(domainEvent.Submission, Arg.Any<CancellationToken>())
            .Returns(occasionSubmission);

        // Act
        await _handler.HandleAsync(domainEvent);

        // Assert
        await _repository.Received(1).SaveAsync(occasionSubmission, Arg.Any<CancellationToken>());
        Assert.Equal(SubmissionStatus.Rejected, occasionSubmission.Status);
    }

    [Fact]
    public async Task HandleAsync_ShouldLogError_WhenExceptionIsThrown()
    {
        // Arrange
        var domainEvent = new ModerationCaseClosedDomainEvent(
            ModerationCaseId.New(),
            new OccasionSubmissionId(Guid.NewGuid()),
            DateTime.UtcNow,
            true,
            null);

        _repository
            .When(r => r.GetByIdAsync(Arg.Any<OccasionSubmissionId>(), Arg.Any<CancellationToken>()))
            .Do(x => throw new Exception("Test exception"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.HandleAsync(domainEvent));
    }
}