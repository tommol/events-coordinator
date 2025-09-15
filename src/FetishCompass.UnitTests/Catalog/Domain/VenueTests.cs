using AutoFixture;
using FetishCompass.Domain;
using FetishCompass.Domain.Catalog.Model;

namespace FetishCompass.UnitTests.Catalog.Domain;

public sealed class VenueTests
{
    private readonly IFixture fixture = new Fixture();
    private VenueId NewVenueId() => VenueId.New();
    
    [Fact]
    public void Create_ValidData_Succeeds()
    {
        var id = NewVenueId();
        var name = fixture.Create<string>();
        var address = fixture.Create<Address>();
        var venue = Venue.Create(id,name,address);
        
        Assert.Equal(id, venue.Id);
        Assert.Equal(name, venue.Name);
        Assert.Equal(address, venue.Address);
    }

    [Fact]
    public void UpdateAddress_ValidData_UpdatesProperties()
    {
        var id = NewVenueId();
        var name = fixture.Create<string>();
        var address = fixture.Create<Address>();
        var newAddress = fixture.Create<Address>();
        var venue = Venue.Create(id,name,address);
        
        venue.UpdateAddress(newAddress);
        
        Assert.Equal(id, venue.Id);
        Assert.Equal(name, venue.Name);
        Assert.Equal(newAddress, venue.Address);
    }
    
    [Fact]
    public void GeoLocate_ValidData_UpdateLocation()
    {
        var id = NewVenueId();
        var name = fixture.Create<string>();
        var address = fixture.Create<Address>();
        var latitude = 46.0;
        var longitude = 20.1;
        var venue = Venue.Create(id,name,address);
        
        venue.GeoLocate(latitude, longitude);
        
        Assert.Equal(id, venue.Id);
        Assert.Equal(name, venue.Name);
        Assert.NotNull(venue.GeoLocation);
        Assert.Equal(latitude, venue.GeoLocation.Latitude);
        Assert.Equal(longitude, venue.GeoLocation.Longitude);
    }
}