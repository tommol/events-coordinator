using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Catalog.DomainEvents;

public record OccasionVenueChangedDomainEvent(
    OccasionId OccasionId,
    VenueId Venue) : DomainEvent;