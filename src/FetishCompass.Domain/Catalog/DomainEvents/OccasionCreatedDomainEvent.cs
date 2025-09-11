using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Domain.IAM;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Catalog.DomainEvents;

public record OccasionCreatedDomainEvent(
    OccasionId OccasionId,
    string Title,
    string Description,
    OrganizerAccountId Organizer,
    VenueId Venue,
    LocalDateTime OccasionStart,
    LocalDateTime OccasionEnd)
    : DomainEvent;