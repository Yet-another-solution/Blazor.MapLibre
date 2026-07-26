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

/// <summary>
/// Converter for a OneOf pair where the second branch isn't specifically JsonArray (e.g.
/// <c>OneOf&lt;double, PaddingOptions&gt;</c>) - distinguishes branches by JSON token type
/// (an object deserializes as T2, anything else as T1).
/// </summary>
public class OneOfJsonConverter<T1, T2> : JsonConverter<OneOf<T1, T2>>
{
    public override OneOf<T1, T2> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            var t2 = JsonSerializer.Deserialize<T2>(ref reader, options);
            return OneOf<T1, T2>.FromT1(t2!);
        }

        var t1 = JsonSerializer.Deserialize<T1>(ref reader, options);
        return OneOf<T1, T2>.FromT0(t1!);
    }

    public override void Write(Utf8JsonWriter writer, OneOf<T1, T2> value, JsonSerializerOptions options)
    {
        value.Switch(
            v1 => JsonSerializer.Serialize(writer, v1, options),
            v2 => JsonSerializer.Serialize(writer, v2, options)
        );
    }
}