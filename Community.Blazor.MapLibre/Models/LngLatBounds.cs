namespace Community.Blazor.MapLibre.Models;

/// <summary>
/// Represents bounds for geographical coordinates.
/// </summary>
public class LngLatBounds
{
    public required LngLat Southwest { get; set; }
    public required LngLat Northeast { get; set; }
}