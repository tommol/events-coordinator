using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Catalog.DomainEvents;

public record OccasionScheduleUpdatedDomainEvent(
    OccasionId OccasionId,
    OccasionSchedule Schedule) : DomainEvent;