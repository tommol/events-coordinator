using System.Diagnostics.CodeAnalysis;

namespace FetishCompass.Domain.Moderation.Model;

public readonly record struct ModerationCaseId(Guid Value)
    : ISpanFormattable, ISpanParsable<ModerationCaseId>
{
    public static ModerationCaseId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();

    // ---- ISpanFormattable ----
    public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString(format, formatProvider);

    public bool TryFormat(Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => Value.TryFormat(destination, out charsWritten, format);

    // ---- ISpanParsable<ModerationCaseId> (wymagane metody z ReadOnlySpan<char>) ----
    public static ModerationCaseId Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => new(Guid.Parse(s));

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out ModerationCaseId result)
    {
        if (Guid.TryParse(s, out var g))
        {
            result = new ModerationCaseId(g);
            return true;
        }

        result = default;
        return false;
    }


    public static ModerationCaseId Parse(string s, IFormatProvider? provider = null)
        => new(Guid.Parse(s));

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out ModerationCaseId result)
    {
        if (Guid.TryParse(s, out var g))
        {
            result = new ModerationCaseId(g);
            return true;
        }

        result = default;
        return false;
    }

    public static implicit operator Guid(ModerationCaseId id) => id.Value;
    public static explicit operator ModerationCaseId(Guid value) => new(value);
}