using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using OneOf;

namespace Community.Blazor.MapLibre.Converter;

public class OneOfJsonConverter<T1> : JsonConverter<OneOf<T1, JsonArray>>
{
    public override OneOf<T1, JsonArray> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            var t1 = JsonSerializer.Deserialize<T1>(ref reader, options);
            return OneOf<T1, JsonArray>.FromT0(t1!);
        }

        throw new NotImplementedException("This converter is only intended for serialization.");
    }

    public override void Write(Utf8JsonWriter writer, OneOf<T1, JsonArray> value, JsonSerializerOptions options)
    {
        value.Switch(
            v1 => JsonSerializer.Serialize(writer, v1, options),
            v2 => JsonSerializer.Serialize(writer, v2, options)
        );
    }
}
