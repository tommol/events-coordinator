using AutoFixture;
using FetishCompass.Application.Catalog.Commands.Handlers;
using FetishCompass.Application.Catalog.EventHandlers.Domain;
using FetishCompass.Application.Catalog.IntegrationEvents;
using FetishCompass.Domain;
using FetishCompass.Domain.Catalog.DomainEvents;
using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.IAM;
using FetishCompass.Shared.Application.IntegrationEvents;
using FetishCompass.Shared.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FetishCompass.UnitTests.Catalog.Application.DomainEvents;

public sealed class OccasionCreatedDomainEventHandlerTests
{
    [Fact]
    public async Task Handle_OccasionExists_PublishEvent()
    {
        var fixture = new Fixture();
        var repo = Substitute.For<IAggregateRepository<Occasion, OccasionId>>();
        var publisher = Substitute.For<IIntegrationEventPublisher>();
        var logger = Substitute.For<ILogger<OccasionCreatedDomainEventHandler>>();
        var handler = new OccasionCreatedDomainEventHandler(repo, publisher,logger);
        var start = fixture.Create<DateTime>();
        var id = Guid.NewGuid();
        var occasion = Occasion.Create(
            (OccasionId)id,
            "Tytuł test",
            "Opis test",
            OccasionSchedule.Create(
                LocalDateTime.Create(DateOnly.FromDateTime(start), TimeOnly.MinValue, "Europe/Warsaw"),
                LocalDateTime.Create(DateOnly.FromDateTime(start), TimeOnly.MaxValue, "Europe/Warsaw")),
            OccasionStatus.Draft,
            (OrganizerAccountId)Guid.NewGuid(),
            (VenueId)Guid.NewGuid()
        );
        repo.GetByIdAsync((OccasionId)id, Arg.Any<CancellationToken>()).Returns(occasion);
        var domainEvent = (OccasionCreatedDomainEvent)occasion.DomainEvents.Single(x=>x.GetType() == typeof(OccasionCreatedDomainEvent));
        
        await handler.HandleAsync(domainEvent);
        
        await repo.Received(1).GetByIdAsync(occasion.Id, Arg.Any<CancellationToken>());
        await publisher.Received(1).PublishAsync(Arg.Is<OccasionCreatedIntegrationEvent>(
                e=>e.OccasionId == occasion.Id),
            Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Handle_OccasionExists_ThrowsLogsAndNotPublish()
    {
        var fixture = new Fixture();
        var repo = Substitute.For<IAggregateRepository<Occasion, OccasionId>>();
        var publisher = Substitute.For<IIntegrationEventPublisher>();
        var logger = Substitute.For<ILogger<OccasionCreatedDomainEventHandler>>();
        var handler = new OccasionCreatedDomainEventHandler(repo, publisher,logger);
        var start = fixture.Create<DateTime>();
        var id = Guid.NewGuid();
        var occasion = Occasion.Create(
            (OccasionId)id,
            "Tytuł test",
            "Opis test",
            OccasionSchedule.Create(
                LocalDateTime.Create(DateOnly.FromDateTime(start), TimeOnly.MinValue, "Europe/Warsaw"),
                LocalDateTime.Create(DateOnly.FromDateTime(start), TimeOnly.MaxValue, "Europe/Warsaw")),
            OccasionStatus.Draft,
            (OrganizerAccountId)Guid.NewGuid(),
            (VenueId)Guid.NewGuid()
        );
        repo.GetByIdAsync((OccasionId)id, Arg.Any<CancellationToken>()).Returns((Occasion?)null);
        var domainEvent = (OccasionCreatedDomainEvent)occasion.DomainEvents.Single(x=>x.GetType() == typeof(OccasionCreatedDomainEvent));
        
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(domainEvent));
        
        await repo.Received(1).GetByIdAsync(occasion.Id, Arg.Any<CancellationToken>());
        await publisher.DidNotReceive().PublishAsync(Arg.Is<OccasionCreatedIntegrationEvent>(
                e=>e.OccasionId == occasion.Id),
            Arg.Any<CancellationToken>());
    }
}