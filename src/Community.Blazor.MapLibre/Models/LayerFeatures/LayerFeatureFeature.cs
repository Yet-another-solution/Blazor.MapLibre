using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Converter;
using Community.Blazor.MapLibre.Models.Feature;

namespace Community.Blazor.MapLibre.Models.LayerFeatures;

public class LayerFeatureFeature : LayerFeature
{
	[JsonPropertyName("id")]
	[JsonConverter(typeof(StringOrNumberConverter))]
	public string? Id { get; set; }

	[JsonPropertyName("geometry")]
	public required IGeometry Geometry { get; set; }

	[JsonPropertyName("properties")]
	public Dictionary<string, object>? Properties { get; set; }
	
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public override LngLatBounds GetBounds() => Geometry.GetBounds();
}