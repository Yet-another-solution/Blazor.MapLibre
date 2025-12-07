using System.Text.Json.Serialization;
using Community.Blazor.MapLibre.Models.Feature;

namespace Community.Blazor.MapLibre.Models.LayerFeatures;

public class LayerFeatureCollection : LayerFeature
{
	[JsonPropertyName("features")]
	public List<IFeature> Features { get; set; } = [];

	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public override LngLatBounds GetBounds()
	{
		if (Features.Count == 0)
		{
			return new LngLatBounds
			{
				Southwest = new LngLat(0, 0),
				Northeast = new LngLat(0, 0)
			};
		}

		var bounds = Features[0].GetBounds();
		for (var i = 1; i < Features.Count; i++)
		{
			bounds.Extend(Features[i].GetBounds());
		}

		return bounds;
	}
}