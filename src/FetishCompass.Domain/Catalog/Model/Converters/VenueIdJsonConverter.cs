using System.Text.Json;
using System.Text.Json.Serialization;

namespace FetishCompass.Domain.Catalog.Model.Converters;

public sealed class VenueIdJsonConverter : JsonConverter<VenueId>
{
    public override VenueId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(Guid.Parse(reader.GetString()!));

    public override void Write(Utf8JsonWriter writer, VenueId value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.Value);
}