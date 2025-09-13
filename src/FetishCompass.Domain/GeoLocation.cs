namespace FetishCompass.Domain;

public sealed class GeoLocation : IEquatable<GeoLocation>
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    
    public GeoLocation(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90.");
        if (longitude < -180 || longitude > 180)
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180.");

        Latitude = latitude;
        Longitude = longitude;
    }

    public bool Equals(GeoLocation? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is GeoLocation other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Latitude, Longitude);
    }
}