namespace Community.Blazor.MapLibre.Models.Source;

/// <summary>
/// Represents a GeoJSON source. GeoJSON sources provide either inline GeoJSON data or a URL to a GeoJSON file.
/// They can support clustering and other custom behaviors for point features.
/// </summary>
public class GeoJSONSource : SourceBase
{
    /// <inheritdoc />
    public override string Type => "geojson";

    /// <summary>
    /// The GeoJSON data, either as an inline object or a URL to an external GeoJSON file. Required.
    /// </summary>
    public object Data { get; set; } = null!;
}