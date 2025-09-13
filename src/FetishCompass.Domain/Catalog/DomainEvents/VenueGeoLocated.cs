using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Catalog.DomainEvents;

public sealed record VenueGeoLocated(VenueId VenueId, GeoLocation GeoLocation) : DomainEvent;