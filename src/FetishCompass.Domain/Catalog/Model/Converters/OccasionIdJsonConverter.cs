using System.Text.Json;
using System.Text.Json.Serialization;

namespace FetishCompass.Domain.Catalog.Model.Converters;

public sealed class OccasionIdJsonConverter : JsonConverter<OccasionId>
{
    public override OccasionId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(Guid.Parse(reader.GetString()!));

    public override void Write(Utf8JsonWriter writer, OccasionId value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.Value);
}