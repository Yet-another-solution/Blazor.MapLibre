using System.Text.Json;
using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Models.Feature;
using OneOf;

namespace Community.Blazor.MapLibre.Converter;

/// <summary>
/// JSON converter for GeoJsonSource Data property that supports both IFeature objects and URL strings.
/// This allows GeoJSON data to be provided either inline or as a reference to an external GeoJSON file.
/// </summary>
public class GeoJsonDataConverter : JsonConverter<OneOf<IFeature, string>>
{
    public override OneOf<IFeature, string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // If it's a string, it's a URL
        if (reader.TokenType == JsonTokenType.String)
        {
            var url = reader.GetString();
            return OneOf<IFeature, string>.FromT1(url!);
        }

        // Otherwise, it's an IFeature object
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            var feature = JsonSerializer.Deserialize<IFeature>(ref reader, options);
            return OneOf<IFeature, string>.FromT0(feature!);
        }

        throw new JsonException($"Unexpected token type: {reader.TokenType}. Expected String or StartObject.");
    }

    public override void Write(Utf8JsonWriter writer, OneOf<IFeature, string> value, JsonSerializerOptions options)
    {
        value.Switch(
            feature => JsonSerializer.Serialize(writer, feature, options),
            url => writer.WriteStringValue(url)
        );
    }
}
