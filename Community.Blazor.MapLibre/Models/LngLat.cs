using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models;

/// <summary>
/// Represents a geographical coordinate (longitude and latitude).
/// </summary>
public class LngLat
{
    [JsonPropertyName("lng")]
    public double Longitude { get; set; }
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }

    /// <summary>
    /// Default constructor. Allows initialization with object initializers.
    /// </summary>
    public LngLat()
    {
        Longitude = 0;
        Latitude = 0;
    }

    /// <summary>
    /// Constructor that initializes the coordinate with provided longitude and latitude values.
    /// </summary>
    /// <param name="longitude">The longitude of the coordinate.</param>
    /// <param name="latitude">The latitude of the coordinate.</param>
    public LngLat(double longitude, double latitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }
}