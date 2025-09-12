using System.Diagnostics.CodeAnalysis;

namespace FetishCompass.Domain.Moderation.Model;

public readonly record struct OccasionSubmissionId(Guid Value)
    : ISpanFormattable, ISpanParsable<OccasionSubmissionId>
{
    public static OccasionSubmissionId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();

    // ---- ISpanFormattable ----
    public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString(format, formatProvider);

    public bool TryFormat(Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => Value.TryFormat(destination, out charsWritten, format);

    // ---- ISpanParsable<OccasionSubmissionId> (wymagane metody z ReadOnlySpan<char>) ----
    public static OccasionSubmissionId Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => new(Guid.Parse(s));

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out OccasionSubmissionId result)
    {
        if (Guid.TryParse(s, out var g))
        {
            result = new OccasionSubmissionId(g);
            return true;
        }
        result = default;
        return false;
    }


    public static OccasionSubmissionId Parse(string s, IFormatProvider? provider = null)
        => new(Guid.Parse(s));

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out OccasionSubmissionId result)
    {
        if (Guid.TryParse(s, out var g))
        {
            result = new OccasionSubmissionId(g);
            return true;
        }
        result = default;
        return false;
    }

    public static implicit operator Guid(OccasionSubmissionId id) => id.Value;
    public static explicit operator OccasionSubmissionId(Guid value) => new(value);
}