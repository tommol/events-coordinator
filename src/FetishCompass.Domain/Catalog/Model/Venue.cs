using System.Diagnostics.CodeAnalysis;
using FetishCompass.Domain.Catalog.DomainEvents;
using FetishCompass.Shared.Domain;

namespace FetishCompass.Domain.Catalog.Model;

public class Venue : AggregateRoot<VenueId>
{
    public string Name { get; private set; }
    public Address Address { get; private set; }
    public GeoLocation? GeoLocation { get; private set; }

    [ExcludeFromCodeCoverage]
    private Venue()
    {

    }

    private Venue(VenueId id, string name, Address address) : base(id)
    {
        Name = name;
        Address = address;
    }

    public static Venue Create(VenueId id, string name, Address address)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        var venue = new Venue(id, name, address);
        venue.ApplyChange( new VenueCreatedDomainEvent(id, name, address));
        venue.MarkAsModified();
        return venue;
    }

    public void GeoLocate(double latitude, double longitude)
    {
        var geoLocation = new GeoLocation(latitude, longitude);
        ApplyChange(new VenueGeoLocated(Id, geoLocation));
        MarkAsModified();
    }
    
    public void UpdateAddress(Address newAddress)
    {
        if (newAddress == null) throw new ArgumentNullException(nameof(newAddress));
        if (newAddress.Equals(Address)) return;
        ApplyChange(new VenueAddressUpdated(Id, newAddress));
        MarkAsModified();
    }

    private void Apply(VenueCreatedDomainEvent domainEvent)
    {
        Id = domainEvent.VenueId;
        Name = domainEvent.Name;
        Address = domainEvent.Address;
    }
    
    private void Apply(VenueGeoLocated domainEvent)
    {
        GeoLocation = domainEvent.GeoLocation;
    }

    private void Apply(VenueAddressUpdated domainEvent)
    {
        Address = domainEvent.NewAddress;
    }
}
