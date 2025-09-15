using FetishCompass.Domain;
using System;
using Xunit;

namespace FetishCompass.UnitTests.Domain;

public sealed class GeoLocationTests
{
    [Theory]
    [InlineData(-90, 0)]
    [InlineData(90, 0)]
    [InlineData(0, -180)]
    [InlineData(0, 180)]
    [InlineData(45.123, 12.456)]
    public void Constructor_ValidCoordinates_SetsProperties(double latitude, double longitude)
    {
        var geo = new GeoLocation(latitude, longitude);
        Assert.Equal(latitude, geo.Latitude);
        Assert.Equal(longitude, geo.Longitude);
    }

    [Theory]
    [InlineData(-91, 0)]
    [InlineData(91, 0)]
    public void Constructor_InvalidLatitude_Throws(double latitude, double longitude)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new GeoLocation(latitude, longitude));
    }

    [Theory]
    [InlineData(0, -181)]
    [InlineData(0, 181)]
    public void Constructor_InvalidLongitude_Throws(double latitude, double longitude)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new GeoLocation(latitude, longitude));
    }

    [Fact]
    public void Equals_SameValues_ReturnsTrue()
    {
        var a = new GeoLocation(10, 20);
        var b = new GeoLocation(10, 20);
        Assert.True(a.Equals(b));
        Assert.True(a.Equals((object)b));
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        var a = new GeoLocation(10, 20);
        var b = new GeoLocation(11, 20);
        var c = new GeoLocation(10, 21);
        Assert.False(a.Equals(b));
        Assert.False(a.Equals(c));
        Assert.False(a.Equals(null));
        Assert.False(a.Equals((object)null));
    }
}