using System.Text.Json;
using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Feature.Dto;

/// <summary>
/// DTO for GeoJSON FeatureCollection, used specifically for JSInterop deserialization
/// </summary>
public class JsFeatureCollection
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "FeatureCollection";

    [JsonPropertyName("features")]
    public List<JsFeature> Features { get; set; } = [];

    /// <summary>
    /// Converts the DTO to a domain model FeatureCollection
    /// </summary>
    public FeatureCollection ToFeatureCollection()
    {
        var result = new FeatureCollection();
        if (Features != null)
        {
            result.Features = Features.Select(f => f.ToFeatureFeature()).Cast<IFeature>().ToList();
        }
        return result;
    }
}

/// <summary>
/// DTO for GeoJSON Feature, used specifically for JSInterop deserialization
/// </summary>
public class JsFeature
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "Feature";

    [JsonPropertyName("geometry")]
    public JsGeometry? Geometry { get; set; }

    [JsonPropertyName("properties")]
    public JsonElement? Properties { get; set; }

    /// <summary>
    /// Converts the DTO to a domain model FeatureFeature
    /// </summary>
    public FeatureFeature ToFeatureFeature()
    {
        var properties = new Dictionary<string, object>();
        
        // Convert JsonElement properties to a dictionary if they exist
        if (Properties.HasValue && Properties.Value.ValueKind == JsonValueKind.Object)
        {
            foreach (var prop in Properties.Value.EnumerateObject())
            {
                properties[prop.Name] = ConvertJsonElementToObject(prop.Value);
            }
        }
        
        return new FeatureFeature
        {
            Id = Id,
            Geometry = Geometry?.ToGeometry() ?? new PointGeometry { Coordinates = new double[] { 0, 0 } },
            Properties = properties.Count > 0 ? properties : null
        };
    }
    
    /// <summary>
    /// Helper method to convert JsonElement to appropriate .NET type
    /// </summary>
    private object ConvertJsonElementToObject(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString() ?? string.Empty,
            JsonValueKind.Number when element.TryGetInt32(out int intValue) => intValue,
            JsonValueKind.Number when element.TryGetInt64(out long longValue) => longValue,
            JsonValueKind.Number => element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            JsonValueKind.Object => element,
            JsonValueKind.Array => element,
            _ => element.ToString()
        };
    }
}

/// <summary>
/// DTO for GeoJSON Geometry, used specifically for JSInterop deserialization
/// </summary>
public class JsGeometry
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "";

    [JsonPropertyName("coordinates")]
    public JsonElement Coordinates { get; set; }

    /// <summary>
    /// Converts the DTO to an appropriate domain model geometry based on the type
    /// </summary>
    public IGeometry ToGeometry()
    {
        return Type switch
        {
            "Point" => new PointGeometry
            {
                Coordinates = JsonSerializer.Deserialize<double[]>(Coordinates.GetRawText())!
            },
            "LineString" => new LineGeometry
            {
                Coordinates = JsonSerializer.Deserialize<double[][]>(Coordinates.GetRawText())!
            },
            "Polygon" => new PolygonGeometry
            {
                Coordinates = JsonSerializer.Deserialize<double[][][]>(Coordinates.GetRawText())!
            },
            _ => throw new NotSupportedException($"Geometry type '{Type}' is not supported")
        };
    }
}
