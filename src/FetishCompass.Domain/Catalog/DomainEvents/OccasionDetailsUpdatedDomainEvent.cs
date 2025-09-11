using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Catalog.DomainEvents;

public record OccasionDetailsUpdatedDomainEvent(
    OccasionId OccasionId,
    string Title,
    string Description) : DomainEvent;