using System.Text.Json;
using System.Text.Json.Serialization;

namespace FetishCompass.Domain;

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