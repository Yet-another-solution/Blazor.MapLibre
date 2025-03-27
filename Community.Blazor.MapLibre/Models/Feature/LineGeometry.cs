using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Feature;

public class LineGeometry : IGeometry
{
    [JsonPropertyName("type")]
    public string Type => "LineString";

    public required double[][] Coordinates { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public LngLatBounds GetBounds()
    {
        if (Coordinates.Length == 0)
        {
            return new LngLatBounds
            {
                Southwest = new LngLat(0, 0),
                Northeast = new LngLat(0, 0)
            };
        }

        var minLng = double.MaxValue;
        var minLat = double.MaxValue;
        var maxLng = double.MinValue;
        var maxLat = double.MinValue;

        foreach (var coordinates in Coordinates)
        {
            minLng = Math.Min(minLng, coordinates[0]);
            minLat = Math.Min(minLat, coordinates[1]);
            maxLng = Math.Max(maxLng, coordinates[0]);
            maxLat = Math.Max(maxLat, coordinates[1]);
        }

        return new LngLatBounds
        {
            Southwest = new LngLat(minLng, minLat),
            Northeast = new LngLat(maxLng, maxLat)
        };
    }
}
