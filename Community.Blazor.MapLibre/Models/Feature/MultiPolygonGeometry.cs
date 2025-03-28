using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Feature;

public class MultiPolygonGeometry : IGeometry
{
    [JsonPropertyName("type")]
    public GeometryType Type => GeometryType.MultiPolygon;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public LngLatBounds GetBounds()
    {
        throw new NotImplementedException();
    }
}
