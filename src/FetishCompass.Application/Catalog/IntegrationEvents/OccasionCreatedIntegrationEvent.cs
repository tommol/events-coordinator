using FetishCompass.Shared.Application.IntegrationEvents;

namespace FetishCompass.Application.Catalog.IntegrationEvents;

public sealed record OccasionCreatedIntegrationEvent(
    Guid OccasionId,
    string Title,
    string Description,
    DateTime LocalStart,
    DateTime LocalEnd,
    string TimeZone,
    Guid Organizer,
    Guid Venue) : IntegrationEvent;
