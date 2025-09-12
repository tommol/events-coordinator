using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Catalog.DomainEvents;

public record OccasionMarkedAsReadyDomainEvent(OccasionId OccasionId) : DomainEvent;