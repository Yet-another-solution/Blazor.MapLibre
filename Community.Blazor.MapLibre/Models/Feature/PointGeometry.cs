using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Feature;

public class PointGeometry : IGeometry
{
    [JsonPropertyName("type")]
    public GeometryType Type => GeometryType.Point;

    [JsonPropertyName("coordinates")]
    public required double[] Coordinates { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public LngLatBounds GetBounds()
    {
        return new LngLatBounds
        {
            Southwest = new LngLat(Coordinates[0], Coordinates[1]),
            Northeast = new LngLat(Coordinates[0], Coordinates[1])
        };
    }
}
