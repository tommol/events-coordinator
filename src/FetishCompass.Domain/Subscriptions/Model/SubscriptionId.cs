using System.Diagnostics.CodeAnalysis;

namespace FetishCompass.Domain.Subscriptions.Model;

public readonly record struct SubscriptionId(Guid Value)
    : ISpanFormattable, ISpanParsable<SubscriptionId>
{
    public static SubscriptionId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();

    // ---- ISpanFormattable ----
    public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString(format, formatProvider);

    public bool TryFormat(Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => Value.TryFormat(destination, out charsWritten, format);

    // ---- ISpanParsable<OccasionId> (wymagane metody z ReadOnlySpan<char>) ----
    public static SubscriptionId Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => new(Guid.Parse(s));

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out SubscriptionId result)
    {
        if (Guid.TryParse(s, out var g))
        {
            result = new SubscriptionId(g);
            return true;
        }
        result = default;
        return false;
    }


    public static SubscriptionId Parse(string s, IFormatProvider? provider = null)
        => new(Guid.Parse(s));

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out SubscriptionId result)
    {
        if (Guid.TryParse(s, out var g))
        {
            result = new SubscriptionId(g);
            return true;
        }
        result = default;
        return false;
    }

    public static implicit operator Guid(SubscriptionId id) => id.Value;
    public static explicit operator SubscriptionId(Guid value) => new(value);
}