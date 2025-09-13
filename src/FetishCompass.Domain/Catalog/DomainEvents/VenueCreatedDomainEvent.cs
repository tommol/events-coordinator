using FetishCompass.Domain.Catalog.Model;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Catalog.DomainEvents;

public sealed record VenueCreatedDomainEvent(VenueId VenueId, string Name, Address Address) : DomainEvent;