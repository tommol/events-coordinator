using FetishCompass.Domain;
using System;
using Xunit;

namespace FetishCompass.UnitTests.Domain;

public sealed class AddressTests
{
    [Fact]
    public void Create_ValidArguments_SetsProperties()
    {
        var address = Address.Create("Poland", "00-001", "Warsaw", "Main St 1");
        Assert.Equal("Poland", address.Country);
        Assert.Equal("00-001", address.PostalCode);
        Assert.Equal("Warsaw", address.City);
        Assert.Equal("Main St 1", address.Street);
    }

    [Theory]
    [InlineData(null, "00-001", "Warsaw", "Main St 1", "country")]
    [InlineData("Poland", null, "Warsaw", "Main St 1", "postalCode")]
    [InlineData("Poland", "00-001", null, "Main St 1", "city")]
    [InlineData("Poland", "00-001", "Warsaw", null, "street")]
    public void Create_NullArguments_ThrowsArgumentNullException(string country, string postalCode, string city, string street, string paramName)
    {
        var ex = Assert.Throws<ArgumentNullException>(() => Address.Create(country, postalCode, city, street));
        Assert.Equal(paramName, ex.ParamName);
    }

    [Fact]
    public void Equals_SameValues_ReturnsTrue()
    {
        var a = Address.Create("Poland", "00-001", "Warsaw", "Main St 1");
        var b = Address.Create("Poland", "00-001", "Warsaw", "Main St 1");
        Assert.True(a.Equals(b));
        Assert.True(a.Equals((object)b));
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        var a = Address.Create("Poland", "00-001", "Warsaw", "Main St 1");
        var b = Address.Create("Poland", "00-002", "Warsaw", "Main St 1");
        var c = Address.Create("Poland", "00-001", "Krakow", "Main St 1");
        var d = Address.Create("Poland", "00-001", "Warsaw", "Other St 2");
        Assert.False(a.Equals(b));
        Assert.False(a.Equals(c));
        Assert.False(a.Equals(d));
        Assert.False(a.Equals(null));
        Assert.False(a.Equals((object)null));
    }
}