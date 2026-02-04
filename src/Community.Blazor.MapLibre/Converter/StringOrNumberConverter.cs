using System.Text.Json;
using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Converter;

public class StringOrNumberConverter : JsonConverter<string>
{
	public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return reader.TokenType switch
		{
			JsonTokenType.String => reader.GetString(),
			JsonTokenType.Number => reader.GetInt32().ToString(),
			_ => throw new ArgumentOutOfRangeException(),
		};
	}

	public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value);
	}
}
