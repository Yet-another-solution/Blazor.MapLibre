using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Feature;

public class PolygonGeometry : IGeometry
{
    [JsonPropertyName("type")]
    public GeometryType Type => GeometryType.Polygon;

    /// <summary>
    /// Coordinates of a Polygon are an array of LinearRing coordinate arrays.
    /// The first element in the array represents the exterior ring.
    /// Any subsequent elements represent interior rings (or holes).
    /// <example>
    /// No holes:
    /// <code>
    /// {
    ///   "type": "Polygon",
    ///   "coordinates": [
    ///     [ [100.0, 0.0], [101.0, 0.0], [101.0, 1.0], [100.0, 1.0], [100.0, 0.0] ]
    ///   ]
    /// }
    /// </code>
    /// </example>
    /// <example>
    /// With holes:
    /// <code>
    /// {
    ///   "type": "Polygon",
    ///   "coordinates": [
    ///     [ [100.0, 0.0], [101.0, 0.0], [101.0, 1.0], [100.0, 1.0], [100.0, 0.0] ],
    ///     [ [100.2, 0.2], [100.8, 0.2], [100.8, 0.8], [100.2, 0.8], [100.2, 0.2] ]
    ///   ]
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public required double[][][] Coordinates { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public LngLatBounds GetBounds()
    {
        if (Coordinates.Length == 0 || Coordinates[0].Length == 0 || Coordinates[0][0].Length == 0)
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

        foreach (var group in Coordinates)
        {
            foreach (var coordinate in group)
            {
                var lng = coordinate[0];
                var lat = coordinate[1];

                minLng = Math.Min(minLng, lng);
                minLat = Math.Min(minLat, lat);
                maxLng = Math.Max(maxLng, lng);
                maxLat = Math.Max(maxLat, lat);
            }
        }

        return new LngLatBounds()
        {
            Northeast = new LngLat(maxLng, maxLat),
            Southwest = new LngLat(minLng, minLat)
        };
    }
}
