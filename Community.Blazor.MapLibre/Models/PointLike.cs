namespace Community.Blazor.MapLibre.Models;

/// <summary>
/// Represents a point-like object as either an object with named properties (X, Y)
/// or an array-like structure (two numbers).
/// </summary>
public class PointLike
{
    public double X { get; set; }
    public double Y { get; set; }

    public PointLike(double x, double y)
    {
        X = x;
        Y = y;
    }

    public static PointLike FromArray(double[] coordinates)
    {
        if (coordinates.Length != 2)
            throw new ArgumentException("Array must have exactly two elements.");
        return new PointLike(coordinates[0], coordinates[1]);
    }

    public override string ToString() => $"PointLike({X}, {Y})";
}