using System.Diagnostics.CodeAnalysis;

namespace FetishCompass.Domain.Catalog.Model;

public readonly record struct VenueId(Guid Value)
    : ISpanFormattable, ISpanParsable<VenueId>
{
    public static VenueId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();

    // ---- ISpanFormattable ----
    public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString(format, formatProvider);

    public bool TryFormat(Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => Value.TryFormat(destination, out charsWritten, format);

    // ---- ISpanParsable<VenueId> (wymagane metody z ReadOnlySpan<char>) ----
    public static VenueId Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => new(Guid.Parse(s));

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out VenueId result)
    {
        if (Guid.TryParse(s, out var g))
        {
            result = new VenueId(g);
            return true;
        }
        result = default;
        return false;
    }


    public static VenueId Parse(string s, IFormatProvider? provider = null)
        => new(Guid.Parse(s));

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out VenueId result)
    {
        if (Guid.TryParse(s, out var g))
        {
            result = new VenueId(g);
            return true;
        }
        result = default;
        return false;
    }

    public static implicit operator Guid(VenueId id) => id.Value;
    public static explicit operator VenueId(Guid value) => new(value);
}