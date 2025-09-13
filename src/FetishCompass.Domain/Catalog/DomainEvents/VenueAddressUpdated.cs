using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Catalog.DomainEvents;

public sealed record VenueAddressUpdated(VenueId VenueId, Address NewAddress) : DomainEvent;