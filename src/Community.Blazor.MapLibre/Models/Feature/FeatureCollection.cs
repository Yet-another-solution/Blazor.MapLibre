using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Feature;

public class FeatureCollection : IFeature
{
    [JsonPropertyName("type")]
    public string Type => "FeatureCollection";

    [JsonPropertyName("features")]
    public List<IFeature> Features { get; set; } = [];

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public LngLatBounds GetBounds()
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
