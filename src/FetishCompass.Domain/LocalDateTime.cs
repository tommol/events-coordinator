using System.Text.Json;
using System.Text.Json.Serialization;

namespace FetishCompass.Domain;

/// <summary>
/// Lokalna data+czas wydarzenia + identyfikator strefy (preferuj IANA, np. "Europe/Warsaw").
/// Zapewnia jednoznaczną konwersję do UTC.
/// </summary>
public sealed class LocalDateTime : IEquatable<LocalDateTime>
{
    public DateOnly Date { get; }
    public TimeOnly Time { get; }
    public string TimeZoneId { get; }

    private LocalDateTime(DateOnly date, TimeOnly time, string timeZoneId)
    {
        this.Date = date;
        this.Time = time;
        this.TimeZoneId = timeZoneId;
    }

    /// <summary>
    /// Fabryka z walidacją strefy i lokalnego „slotu” czasu (odrzuca czasy nieistniejące w DST).
    /// Jeśli czas jest dwuznaczny (ambiguous), domyślnie wybiera późniejsze przesunięcie (częściej oczekiwane).
    /// </summary>
    public static LocalDateTime Create(
        DateOnly date,
        TimeOnly time,
        string timeZoneId,
        AmbiguousTimeResolution ambiguous = AmbiguousTimeResolution.Later)
    {
        var tz = ResolveTimeZone(timeZoneId);
        var local = date.ToDateTime(time, DateTimeKind.Unspecified);

        if (tz.IsInvalidTime(local))
            throw new InvalidOperationException($"Local time {local} is invalid in '{timeZoneId}' (DST gap).");

        // Nie zapisujemy offsetu – wybór offsetu nastąpi przy konwersji do UTC.
        // Dla dwuznaczności możemy tylko pamiętać wybraną strategię w konwersji.
        return new LocalDateTime(date, time, tz.Id)
            .WithAmbiguousPolicy(ambiguous);
    }

    // Polityka rozstrzygania dwuznaczności – przechowywana „miękko” (nie trafia do DB).
    private AmbiguousTimeResolution _ambiguous = AmbiguousTimeResolution.Later;
    public LocalDateTime WithAmbiguousPolicy(AmbiguousTimeResolution ambiguous)
    {
        this._ambiguous = ambiguous;
        return this;
    }

    /// <summary>
    /// Punkt w czasie w UTC (DateTimeOffset) wyliczony z lokalnej daty/godziny i strefy.
    /// </summary>
    public DateTimeOffset ToUtcInstant()
    {
        var tz = ResolveTimeZone(this.TimeZoneId);
        var local = this.Date.ToDateTime(this.Time, DateTimeKind.Unspecified);

        if (tz.IsInvalidTime(local))
            throw new InvalidOperationException($"Local time {local} is invalid in '{this.TimeZoneId}' (DST gap).");

        TimeSpan offset;
        if (tz.IsAmbiguousTime(local))
        {
            var offsets = tz.GetAmbiguousTimeOffsets(local);
            offset = this._ambiguous == AmbiguousTimeResolution.Earlier
                ? offsets[0]
                : offsets[^1];
        }
        else
        {
            offset = tz.GetUtcOffset(local);
        }

        var localWithOffset = new DateTimeOffset(local, offset);
        return localWithOffset.ToUniversalTime(); // jednoznaczny UTC
    }

    /// <summary>
    /// Konwersja do czasu w innej strefie (np. dla UI użytkownika).
    /// </summary>
    public DateTimeOffset ToUserZone(string userTimeZoneId)
    {
        var utc = this.ToUtcInstant();
        var userTz = ResolveTimeZone(userTimeZoneId);
        return TimeZoneInfo.ConvertTime(utc, userTz);
    }

    /// <summary>
    /// Zwraca nowy VO z inną strefą (np. korekta w edycji).
    /// </summary>
    public LocalDateTime WithTimeZone(string newTimeZoneId, AmbiguousTimeResolution ambiguous = AmbiguousTimeResolution.Later)
        => Create(this.Date, this.Time, newTimeZoneId, ambiguous);

    /// <summary>
    /// Porównywanie strukturalne (Date, Time, TimeZoneId).
    /// </summary>
    public bool Equals(LocalDateTime? other)
        => other is not null &&
           this.Date == other.Date &&
           this.Time == other.Time &&
           string.Equals(this.TimeZoneId, other.TimeZoneId, StringComparison.Ordinal);

    public override bool Equals(object? obj) => obj is LocalDateTime o && this.Equals(o);
    public override int GetHashCode() => HashCode.Combine(this.Date, this.Time, this.TimeZoneId);

    private static TimeZoneInfo ResolveTimeZone(string timeZoneId)
    {
        // Prefer IANA (Linux/containers). Na Windows w .NET 8+ IANA jest wspierane, ale różnie bywa
        // Gdyby było trzeba wspierać Windows IDs, można tu dodać mapowanie.
        try { return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId); }
        catch (TimeZoneNotFoundException)
        {
            throw new InvalidOperationException($"Unknown time zone id '{timeZoneId}'. Use IANA id e.g. 'Europe/Warsaw'.");
        }
    }
}
public sealed class LocalDateTimeJsonConverter : JsonConverter<LocalDateTime>
{
    public override LocalDateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Oczekujemy: {"date":"2025-09-11","time":"20:00:00","timeZoneId":"Europe/Warsaw"}
        if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();
        DateOnly? date = null;
        TimeOnly? time = null;
        string? tz = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException();
            var name = reader.GetString();
            reader.Read();
            switch (name)
            {
                case "date":
                    date = DateOnly.Parse(reader.GetString()!);
                    break;
                case "time":
                    time = TimeOnly.Parse(reader.GetString()!);
                    break;
                case "timeZoneId":
                    tz = reader.GetString();
                    break;
            }
        }
        if (date is null || time is null || string.IsNullOrWhiteSpace(tz))
            throw new JsonException("Invalid EventLocalDateTime payload.");

        return LocalDateTime.Create(date.Value, time.Value, tz);
    }

    public override void Write(Utf8JsonWriter writer, LocalDateTime value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("date", value.Date.ToString("yyyy-MM-dd"));
        writer.WriteString("time", value.Time.ToString("HH:mm:ss"));
        writer.WriteString("timeZoneId", value.TimeZoneId);
        writer.WriteEndObject();
    }
}