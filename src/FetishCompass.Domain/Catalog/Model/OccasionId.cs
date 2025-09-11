using System.Diagnostics.CodeAnalysis;

namespace FetishCompass.Domain.Catalog.Model;

public readonly record struct OccasionId(Guid Value)
    : ISpanFormattable, ISpanParsable<OccasionId>
{
    public static OccasionId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();

    // ---- ISpanFormattable ----
    public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString(format, formatProvider);

    public bool TryFormat(Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => Value.TryFormat(destination, out charsWritten, format);

    // ---- ISpanParsable<OccasionId> (wymagane metody z ReadOnlySpan<char>) ----
    public static OccasionId Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => new(Guid.Parse(s));

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out OccasionId result)
    {
        if (Guid.TryParse(s, out var g))
        {
            result = new OccasionId(g);
            return true;
        }
        result = default;
        return false;
    }


    public static OccasionId Parse(string s, IFormatProvider? provider = null)
        => new(Guid.Parse(s));

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out OccasionId result)
    {
        if (Guid.TryParse(s, out var g))
        {
            result = new OccasionId(g);
            return true;
        }
        result = default;
        return false;
    }

    public static implicit operator Guid(OccasionId id) => id.Value;
    public static explicit operator OccasionId(Guid value) => new(value);
}