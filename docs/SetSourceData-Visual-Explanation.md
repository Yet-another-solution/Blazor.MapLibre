# SetSourceData Fix - Visual Explanation

## 📊 The Problem Illustrated

```
┌─────────────────────────────────────────────────────────────┐
│  C# Code: SetSourceData Method (ORIGINAL - BROKEN)         │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  await _jsModule.InvokeVoidAsync(                          │
│      "setSourceData",                                       │
│      MapId,                                                 │
│      id,                                                    │
│      source.Data  ←─── ❌ PROBLEM HERE!                   │
│  );                                                         │
│                                                             │
└─────────────────────────────────────────────────────────────┘
                           │
                           │ JS Interop Serialization
                           ▼
┌─────────────────────────────────────────────────────────────┐
│  What happens during serialization:                         │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  1. System.Text.Json sees: OneOf<IFeature, string>        │
│  2. Looks for [JsonConverter] on the TYPE ← Not found!    │
│  3. Tries default serialization                            │
│  4. ❌ FAILS with InvalidOperationException                │
│                                                             │
│  The [JsonConverter] attribute is on the PROPERTY,         │
│  not the TYPE, so it's lost when we extract source.Data!   │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

## ✅ The Solution Illustrated

### Approach: Serialize Full Object, Extract Field

```
┌──────────────────────────────────────────────────────────────┐
│  Step 1: Serialize the ENTIRE GeoJsonSource object          │
├──────────────────────────────────────────────────────────────┤
│                                                              │
│  GeoJsonSource                                               │
│  ┌────────────────────────────────────────┐                 │
│  │ Type: "geojson"                        │                 │
│  │                                        │                 │
│  │ Data: [JsonConverter(GeoJsonDataConverter)]             │
│  │   ┌──────────────────────────────┐    │                 │
│  │   │ OneOf<IFeature, string>      │ ←──┼─ Converter IS   │
│  │   │                              │    │    applied here! │
│  │   │ FeatureCollection {          │    │                 │
│  │   │   Features: [...]            │    │                 │
│  │   │ }                            │    │                 │
│  │   └──────────────────────────────┘    │                 │
│  │                                        │                 │
│  │ Cluster: true                          │                 │
│  └────────────────────────────────────────┘                 │
│                                                              │
│              JsonSerializer.SerializeToNode(source)          │
│                            │                                 │
│                            ▼                                 │
│  ┌────────────────────────────────────────┐                 │
│  │ {                                      │                 │
│  │   "type": "geojson",                   │                 │
│  │   "data": {                ← ✅ Properly converted!      │
│  │     "type": "FeatureCollection",       │                 │
│  │     "features": [...]                  │                 │
│  │   },                                   │                 │
│  │   "cluster": true                      │                 │
│  │ }                                      │                 │
│  └────────────────────────────────────────┘                 │
│                                                              │
└──────────────────────────────────────────────────────────────┘
                           │
                           │
                           ▼
┌──────────────────────────────────────────────────────────────┐
│  Step 2: Extract JUST the "data" field                      │
├──────────────────────────────────────────────────────────────┤
│                                                              │
│  var dataNode = jsonNode["data"];                            │
│                                                              │
│  Result:                                                     │
│  ┌────────────────────────────────────────┐                 │
│  │ {                                      │                 │
│  │   "type": "FeatureCollection",         │ ← ✅ Pure       │
│  │   "features": [                        │    GeoJSON      │
│  │     {                                  │    data only    │
│  │       "type": "Feature",               │                 │
│  │       "geometry": { ... },             │                 │
│  │       "properties": { ... }            │                 │
│  │     }                                  │                 │
│  │   ]                                    │                 │
│  │ }                                      │                 │
│  └────────────────────────────────────────┘                 │
│                                                              │
│  Notice: No "type": "geojson", no "cluster": true           │
│           Just the GeoJSON data JavaScript needs!           │
│                                                              │
└──────────────────────────────────────────────────────────────┘
                           │
                           │ Pass to JavaScript
                           ▼
┌──────────────────────────────────────────────────────────────┐
│  JavaScript: setSourceData function                          │
├──────────────────────────────────────────────────────────────┤
│                                                              │
│  function setSourceData(container, id, data) {               │
│      const source = mapInstances[container].getSource(id);   │
│      source.setData(data);  ← ✅ Works! Valid GeoJSON       │
│  }                                                           │
│                                                              │
└──────────────────────────────────────────────────────────────┘
```

## 🔍 Code Comparison: Before vs After

### Before (Original - 4 lines with using statement)

```csharp
public async ValueTask SetSourceData(string id, GeoJsonSource source)
{
    var json = JsonSerializer.Serialize(source);
    using var jsonDoc = JsonDocument.Parse(json);
    var dataElement = jsonDoc.RootElement.GetProperty("data");

    await _jsModule.InvokeVoidAsync("setSourceData", MapId, id, dataElement);
}
```

**Issues:**
- ❌ Serialize to string (memory allocation)
- ❌ Parse string back to document (CPU overhead)
- ❌ Requires `using` statement (manual disposal)
- ❌ Verbose property access with `GetProperty()`
- ❌ Needs `Clone()` for bulk transactions

### After (Refactored - 2 lines, no using)

```csharp
public async ValueTask SetSourceData(string id, GeoJsonSource source)
{
    var jsonNode = JsonSerializer.SerializeToNode(source);
    var dataNode = jsonNode!["data"];

    await _jsModule.InvokeVoidAsync("setSourceData", MapId, id, dataNode);
}
```

**Benefits:**
- ✅ Direct serialization to node (no string)
- ✅ No manual parsing
- ✅ No `using` statement needed
- ✅ Intuitive indexer syntax `["data"]`
- ✅ No `Clone()` needed for bulk transactions
- ✅ Same functionality, cleaner code

## 🧪 Test Explanations

### Why Tests Pass: The Key Insight

```
When you serialize an object with System.Text.Json:
┌─────────────────────────────────────────────────────────────┐
│  JsonSerializer.Serialize(source)                           │
│  JsonSerializer.SerializeToNode(source)                     │
│                                                             │
│  Both methods RESPECT property-level [JsonConverter]       │
│  attributes during serialization!                           │
└─────────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────┐
│  GeoJsonDataConverter.Write() is called                     │
│                                                             │
│  Handles OneOf<IFeature, string>:                          │
│    • If T0 (IFeature) → serialize as GeoJSON object        │
│    • If T1 (string)   → serialize as URL string            │
└─────────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────┐
│  Result: Properly serialized GeoJSON in the "data" field   │
└─────────────────────────────────────────────────────────────┘
```

### Test Categories & Why They Pass

#### 1. **Serialization Tests**

```csharp
[Fact]
public void GeoJsonSource_Serialization_Should_Apply_Converter_To_FeatureCollection()
{
    var source = new GeoJsonSource { Data = new FeatureCollection { ... } };
    var jsonNode = JsonSerializer.SerializeToNode(source);
    var dataNode = jsonNode!["data"];
    // ✅ PASSES: Converter transforms FeatureCollection → valid GeoJSON
}
```

**Why it passes:** `GeoJsonDataConverter.Write()` is invoked during serialization because the `[JsonConverter]` attribute is on the `Data` property.

#### 2. **Data Extraction Tests**

```csharp
[Fact]
public void Extracted_Data_Should_Not_Contain_Source_Type()
{
    var source = new GeoJsonSource {
        Data = new FeatureCollection { ... },
        Cluster = true,  // ← Source configuration
        ClusterRadius = 50
    };

    var jsonNode = JsonSerializer.SerializeToNode(source);
    var dataNode = jsonNode!["data"];
    var dataJson = dataNode.ToString();

    dataJson.Should().NotContain("cluster");  // ✅ PASSES
    dataJson.Should().NotContain("geojson");  // ✅ PASSES
}
```

**Why it passes:** We extract ONLY the "data" field, leaving behind all source configuration properties.

#### 3. **Complex Geometry Tests**

```csharp
[Fact]
public void Complex_FeatureCollection_Should_Serialize_All_Geometry_Types()
{
    var source = new GeoJsonSource {
        Data = new FeatureCollection {
            Features = [
                new FeatureFeature { Geometry = new PointGeometry { ... } },
                new FeatureFeature { Geometry = new LineGeometry { ... } },
                new FeatureFeature { Geometry = new PolygonGeometry { ... } }
            ]
        }
    };

    // ✅ PASSES: All geometry types serialize correctly
}
```

**Why it passes:** `IFeature` has `[JsonPolymorphic]` attributes that handle different geometry types correctly.

#### 4. **URL String Tests**

```csharp
[Fact]
public void GeoJsonSource_With_URL_String_Should_Serialize_Correctly()
{
    var source = new GeoJsonSource { Data = "https://example.com/data.geojson" };
    var jsonNode = JsonSerializer.SerializeToNode(source);
    var dataNode = jsonNode!["data"];

    dataNode.GetValue<string>().Should().Be("https://example.com/data.geojson");
    // ✅ PASSES
}
```

**Why it passes:** `GeoJsonDataConverter.Write()` handles the `string` case (T1) by writing it as a JSON string value.

#### 5. **Bulk Transaction Tests**

```csharp
[Fact]
public void BulkTransaction_Should_Support_SetSourceData()
{
    var transaction = new BulkTransaction();
    var jsonNode = JsonSerializer.SerializeToNode(source);
    var dataNode = jsonNode!["data"];

    transaction.Add("setSourceData", "source-id", dataNode);
    var json = JsonSerializer.Serialize(transaction.Transactions);

    json.Should().Contain("FeatureCollection");  // ✅ PASSES
}
```

**Why it passes:** `JsonNode` is serializable and maintains its structure when added to a transaction. No `Clone()` needed!

#### 6. **Performance Tests**

```csharp
[Fact]
public void Large_FeatureCollection_Should_Extract_Data_Efficiently()
{
    var features = Enumerable.Range(0, 1000).Select(i => new FeatureFeature { ... });
    var source = new GeoJsonSource { Data = new FeatureCollection { Features = features } };

    var startTime = DateTime.UtcNow;
    var jsonNode = JsonSerializer.SerializeToNode(source);
    var dataNode = jsonNode!["data"];
    var duration = DateTime.UtcNow - startTime;

    duration.Should().BeLessThan(TimeSpan.FromSeconds(2));  // ✅ PASSES
}
```

**Why it passes:** `JsonNode` serialization is efficient and doesn't require string allocation/parsing like `JsonDocument`.

## 📝 Summary

### The Fix Works Because:

1. **Property-level converter IS applied** when serializing the full `GeoJsonSource` object
2. **Extraction preserves the converted value** - the "data" field already has valid GeoJSON
3. **JsonNode/JsonElement serialize correctly** when passed to JavaScript interop
4. **JavaScript receives pure GeoJSON** without source configuration

### Why JsonNode is Better:

| Feature | JsonDocument | JsonNode |
|---------|-------------|----------|
| Lines of code | 4 | 2 |
| Memory efficiency | String + parse | Direct |
| Syntax clarity | `GetProperty()` | `["data"]` |
| Disposal needed | Yes (`using`) | No |
| Clone for transactions | Yes | No |
| Performance | Slower | Faster |

### The Bottom Line:

**Both approaches work, but JsonNode is simpler, more efficient, and more maintainable.**
