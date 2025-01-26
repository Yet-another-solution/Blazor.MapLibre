using System;

namespace Community.Blazor.MapLibre.Models;

/// <summary>
/// Represents a bounding box for geographical coordinates. It allows operations such as
/// extending the bounds, checking points within bounds, and adjusting bounds that cross the anti-meridian.
/// </summary>
public class LngLatBounds
{
    /// <summary>
    /// The southwest corner of the bounding box.
    /// </summary>
    public required LngLat Southwest { get; set; }

    /// <summary>
    /// The northeast corner of the bounding box.
    /// </summary>
    public required LngLat Northeast { get; set; }

    /// <summary>
    /// Extends the bounds to include the specified point.
    /// </summary>
    /// <param name="point">The point to include.</param>
    public void Extend(LngLat point)
    {
        Southwest.Longitude = Math.Min(Southwest.Longitude, point.Longitude);
        Southwest.Latitude = Math.Min(Southwest.Latitude, point.Latitude);
        Northeast.Longitude = Math.Max(Northeast.Longitude, point.Longitude);
        Northeast.Latitude = Math.Max(Northeast.Latitude, point.Latitude);
    }

    /// <summary>
    /// Extends the bounds to include another bounding box.
    /// </summary>
    /// <param name="bounds">The bounds to include.</param>
    public void Extend(LngLatBounds bounds)
    {
        Extend(bounds.Southwest);
        Extend(bounds.Northeast);
    }

    /// <summary>
    /// Returns the center point of the bounding box.
    /// </summary>
    /// <returns>A <see cref="LngLat"/> representing the center of the bounding box.</returns>
    public LngLat GetCenter()
    {
        return new LngLat(
            (Southwest.Longitude + Northeast.Longitude) / 2,
            (Southwest.Latitude + Northeast.Latitude) / 2
        );
    }

    /// <summary>
    /// Returns whether the provided point lies within the bounds.
    /// </summary>
    /// <param name="point">The point to check.</param>
    /// <returns>True if the point is within bounds, otherwise false.</returns>
    public bool Contains(LngLat point)
    {
        var containsLatitude = Southwest.Latitude <= point.Latitude && point.Latitude <= Northeast.Latitude;
        var containsLongitude = Southwest.Longitude <= point.Longitude && point.Longitude <= Northeast.Longitude;

        // Handle anti-meridian crossing
        if (Southwest.Longitude > Northeast.Longitude)
        {
            containsLongitude = Southwest.Longitude <= point.Longitude || point.Longitude <= Northeast.Longitude;
        }

        return containsLatitude && containsLongitude;
    }

    /// <summary>
    /// Converts the bounds to an array in the format of [[swLng, swLat], [neLng, neLat]].
    /// </summary>
    /// <returns>A two-dimensional array representing the bounds.</returns>
    public double[][] ToArray()
    {
        return new[]
        {
            new[] { Southwest.Longitude, Southwest.Latitude },
            new[] { Northeast.Longitude, Northeast.Latitude }
        };
    }

    /// <summary>
    /// Converts the bounds to a string.
    /// </summary>
    /// <returns>A string representation of the bounds.</returns>
    public override string ToString()
    {
        return
            $"LngLatBounds([{Southwest.Longitude}, {Southwest.Latitude}], [{Northeast.Longitude}, {Northeast.Latitude}])";
    }

    /// <summary>
    /// Adjusts bounds that cross the anti-meridian.
    /// </summary>
    /// <returns>An adjusted <see cref="LngLatBounds"/> if crossing occurred, otherwise the original bounds.</returns>
    public LngLatBounds AdjustAntiMeridian()
    {
        if (Southwest.Longitude > Northeast.Longitude)
        {
            return new LngLatBounds
            {
                Southwest = Southwest,
                Northeast = new LngLat(Northeast.Longitude + 360, Northeast.Latitude)
            };
        }

        return this;
    }

    /// <summary>
    /// Creates a new bounds object based on a center point and radius.
    /// </summary>
    /// <param name="center">The center of the bounding box.</param>
    /// <param name="radius">The radius in meters extending from the center.</param>
    /// <returns>A new <see cref="LngLatBounds"/> object.</returns>
    public static LngLatBounds FromLngLat(LngLat center, double radius)
    {
        const double earthCircumference = 40075017; // in meters at the equator
        var latAccuracy = 360 * radius / earthCircumference;
        var lngAccuracy = latAccuracy / Math.Cos(center.Latitude * Math.PI / 180);

        var southwest = new LngLat(center.Longitude - lngAccuracy, center.Latitude - latAccuracy);
        var northeast = new LngLat(center.Longitude + lngAccuracy, center.Latitude + latAccuracy);
        return new LngLatBounds { Southwest = southwest, Northeast = northeast };
    }
}