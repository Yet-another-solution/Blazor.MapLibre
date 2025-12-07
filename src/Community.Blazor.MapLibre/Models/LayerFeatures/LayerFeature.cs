using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.LayerFeatures;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(LayerFeatureCollection), "FeatureCollection")]
[JsonDerivedType(typeof(LayerFeatureFeature), "Feature")]
public abstract class LayerFeature
{
	// TODO: To be added when Layer works for deserialization and serialization
	// [JsonPropertyName("layer")]
	// public required Layer Layer { get; init; }
	
	[JsonPropertyName("source")]
	public required string Source { get; set; }
	
	[JsonPropertyName("state")]
	public Dictionary<string,object> State { get; set; } = [];
	
	/// <summary>
	/// Gets the bounding box of the geometry.
	/// </summary>
	/// <returns>
	/// A <see cref="LngLatBounds"/> object representing the bounding box of the geometry.
	/// </returns>
	public abstract LngLatBounds GetBounds();
}