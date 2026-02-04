using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;

namespace Community.Blazor.MapLibre.Converter;

public class ObjectOrStringConverter<T> : JsonConverter<OneOf<T, string>> where T : class
{
	public override OneOf<T, string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return reader.TokenType switch
		{
			JsonTokenType.StartObject => JsonSerializer.Deserialize<T>(ref reader, options)!,
			JsonTokenType.String => reader.GetString()!,
			_ => throw new ArgumentOutOfRangeException(),
		};
	}

	public override void Write(Utf8JsonWriter writer, OneOf<T, string> value, JsonSerializerOptions options)
	{
		value.Switch(
			t => JsonSerializer.Serialize(writer, t, options),
			str => JsonSerializer.Serialize(writer, str, options)
		);
	}
}