using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models;

/// <summary>
/// Represents a geographical coordinate (longitude and latitude).
/// </summary>
public class LngLat
{
    [JsonPropertyName("lon")]
    public double Longitude { get; set; }
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }
}