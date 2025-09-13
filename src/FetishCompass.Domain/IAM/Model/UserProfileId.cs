using System.Diagnostics.CodeAnalysis;

namespace FetishCompass.Domain.IAM.Model;

public readonly record struct UserProfileId(Guid Value)
    : ISpanFormattable, ISpanParsable<UserProfileId>
{
    public static UserProfileId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();

    // ---- ISpanFormattable ----
    public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString(format, formatProvider);

    public bool TryFormat(Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => Value.TryFormat(destination, out charsWritten, format);

    // ---- ISpanParsable<OccasionId> (wymagane metody z ReadOnlySpan<char>) ----
    public static UserProfileId Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => new(Guid.Parse(s));

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out UserProfileId result)
    {
        if (Guid.TryParse(s, out var g))
        {
            result = new UserProfileId(g);
            return true;
        }
        result = default;
        return false;
    }


    public static UserProfileId Parse(string s, IFormatProvider? provider = null)
        => new(Guid.Parse(s));

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out UserProfileId result)
    {
        if (Guid.TryParse(s, out var g))
        {
            result = new UserProfileId(g);
            return true;
        }
        result = default;
        return false;
    }

    public static implicit operator Guid(UserProfileId id) => id.Value;
    public static explicit operator UserProfileId(Guid value) => new(value);
}