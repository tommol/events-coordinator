using System.Diagnostics.CodeAnalysis;

namespace FetishCompass.Domain.IAM;

public readonly record struct OrganizerAccountId(Guid Value)
    : ISpanFormattable, ISpanParsable<OrganizerAccountId>
{
    public static OrganizerAccountId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();

    // ---- ISpanFormattable ----
    public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString(format, formatProvider);

    public bool TryFormat(Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => Value.TryFormat(destination, out charsWritten, format);

    // ---- ISpanParsable<OccasionId> (wymagane metody z ReadOnlySpan<char>) ----
    public static OrganizerAccountId Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => new(Guid.Parse(s));

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out OrganizerAccountId result)
    {
        if (Guid.TryParse(s, out var g))
        {
            result = new OrganizerAccountId(g);
            return true;
        }
        result = default;
        return false;
    }


    public static OrganizerAccountId Parse(string s, IFormatProvider? provider = null)
        => new(Guid.Parse(s));

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out OrganizerAccountId result)
    {
        if (Guid.TryParse(s, out var g))
        {
            result = new OrganizerAccountId(g);
            return true;
        }
        result = default;
        return false;
    }

    public static implicit operator Guid(OrganizerAccountId id) => id.Value;
    public static explicit operator OrganizerAccountId(Guid value) => new(value);
}
