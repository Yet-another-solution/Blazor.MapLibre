using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Feature;

public class MultiPolygonGeometry : IGeometry
{
    [JsonPropertyName("type")]
    public GeometryType Type => GeometryType.MultiPolygon;

    /// <summary>
    /// Gets or sets the coordinates that define the geometry of the MultiPolygon.
    /// Each coordinate is represented as a four-dimensional array of double values.
    /// The structure follows the GeoJSON format for MultiPolygon geometries.
    /// </summary>
    [JsonPropertyName("coordinates")]
    public required double[][][][] Coordinates { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public LngLatBounds GetBounds()
    {
        throw new NotImplementedException();
    }
}
