using System.Text.Json;
using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Models.Feature;

namespace Community.Blazor.MapLibre.Converter;

public class GeometryConverter : JsonConverter<IGeometry>
{
    public override IGeometry? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (!root.TryGetProperty("type", out var typeProp))
        {
            throw new JsonException("Missing 'type' property.");
        }

        // Parse type using the enum
        var typeStr = typeProp.GetString();
        if (!Enum.TryParse<GeometryType>(typeStr, ignoreCase: false, out var geometryType))
        {
            throw new JsonException($"Unknown geometry type: {typeStr}");
        }

        return geometryType switch
        {
            GeometryType.Point => JsonSerializer.Deserialize<PointGeometry>(root.GetRawText(), options),
            GeometryType.MultiPoint => JsonSerializer.Deserialize<MultiPointGeometry>(root.GetRawText(), options),
            GeometryType.Line => JsonSerializer.Deserialize<LineGeometry>(root.GetRawText(), options),
            GeometryType.MultiLine => JsonSerializer.Deserialize<MultiLineGeometry>(root.GetRawText(), options),
            GeometryType.Polygon => JsonSerializer.Deserialize<PolygonGeometry>(root.GetRawText(), options),
            GeometryType.MultiPolygon => JsonSerializer.Deserialize<MultiPolygonGeometry>(root.GetRawText(), options),
            _ => throw new JsonException($"Unsupported geometry type: {geometryType}")
        };
    }

    public override void Write(Utf8JsonWriter writer, IGeometry value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
    }
}
