namespace FetishCompass.Domain;

public sealed class Address : IEquatable<Address>
{
    private Address(string country, string postalCode, string city, string street)
    {
        this.Country = country ?? throw new ArgumentNullException(nameof(country));
        this.PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));
        this.City = city ?? throw new ArgumentNullException(nameof(city));
        this.Street = street ?? throw new ArgumentNullException(nameof(street));
    }

    public static Address Create(string country, string postalCode, string city, string street)
    {
        return new Address(country, postalCode, city, street);
    }
    
    public string Country { get; init; }
    public string PostalCode { get; init; }
    public string City { get; init; }
    public string Street { get; init; }

    public bool Equals(Address? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Country == other.Country && PostalCode == other.PostalCode && City == other.City && Street == other.Street;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Address other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Country, PostalCode, City, Street);
    }
}