# SetSourceData Fix - Technical Explanation

## The Problem

When calling `SetSourceData`, the method was failing with an `InvalidOperationException` because the `GeoJsonDataConverter` was not being applied during JSON serialization.

### Root Cause

```csharp
// In GeoJsonSource.cs
public class GeoJsonSource : ISource
{
    [JsonPropertyName("data")]
    [JsonConverter(typeof(GeoJsonDataConverter))]  // ← Converter is on the PROPERTY
    public required OneOf<IFeature, string> Data { get; set; }
}

// Original implementation (BROKEN)
public async ValueTask SetSourceData(string id, GeoJsonSource source)
{
    // This passes source.Data directly - the converter attribute is LOST!
    await _jsModule.InvokeVoidAsync("setSourceData", MapId, id, source.Data);
}
```

**Why it failed:**
1. The `[JsonConverter]` attribute is on the **property** in `GeoJsonSource`
2. When we extract `source.Data` and pass it standalone, the property metadata is lost
3. JS Interop serializes the `OneOf<IFeature, string>` without the converter
4. System.Text.Json doesn't know how to handle the union type, causing serialization errors

## The Solution

### Approach 1: String → Parse → Extract (Current Implementation)

```csharp
public async ValueTask SetSourceData(string id, GeoJsonSource source)
{
    // Step 1: Serialize entire source (converter IS applied here)
    var json = JsonSerializer.Serialize(source);

    // Step 2: Parse back to JsonDocument
    using var jsonDoc = JsonDocument.Parse(json);

    // Step 3: Extract just the "data" field
    var dataElement = jsonDoc.RootElement.GetProperty("data");

    // Step 4: Pass to JavaScript
    await _jsModule.InvokeVoidAsync("setSourceData", MapId, id, dataElement);
}
```

**Why this works:**
1. ✅ Serializing the full `GeoJsonSource` applies the converter to the `Data` property
2. ✅ The "data" field in the JSON now has properly serialized GeoJSON
3. ✅ We extract just that field as a `JsonElement`
4. ✅ `JsonElement` serializes correctly when passed to JS Interop

### Approach 2: JsonNode (Simpler Alternative)

```csharp
public async ValueTask SetSourceData(string id, GeoJsonSource source)
{
    // Serialize to JsonNode and extract "data" field
    var jsonNode = JsonSerializer.SerializeToNode(source);
    var dataNode = jsonNode!["data"];

    await _jsModule.InvokeVoidAsync("setSourceData", MapId, id, dataNode);
}
```

**Why this also works:**
1. ✅ `SerializeToNode` respects property converters
2. ✅ Simpler - no string conversion or using statement needed
3. ✅ Direct navigation with `["data"]` indexer
4. ✅ `JsonNode` serializes correctly for JS Interop

**Comparison:**

| Aspect | Approach 1 (JsonDocument) | Approach 2 (JsonNode) |
|--------|--------------------------|---------------------|
| Lines of code | 4 lines | 2 lines |
| Memory efficiency | Requires string allocation + parse | Direct to node structure |
| Clarity | Explicit steps | More concise |
| Performance | Slightly slower (serialize + parse) | Faster (single serialization) |
| Disposal | Requires `using` | No disposal needed |

## Test Explanations

### 1. SetSourceDataTests.cs

These tests verify that the fix correctly extracts and serializes GeoJSON data:

#### **Test: `GeoJsonSource_Serialization_Should_Apply_Converter_To_FeatureCollection`**
```csharp
// What it does:
// 1. Creates a FeatureCollection with a single point feature
// 2. Wraps it in a GeoJsonSource
// 3. Simulates the fix by serializing and extracting the data field
// 4. Verifies the extracted data is valid GeoJSON (not the source wrapper)

// Why it works:
// When we serialize the full GeoJsonSource, the converter on the Data property
// transforms the OneOf<IFeature, string> into proper GeoJSON. The extracted
// "data" field contains only the GeoJSON, not source configuration.
```

#### **Test: `Extracted_Data_Should_Not_Contain_Source_Type`**
```csharp
// What it does:
// Creates a GeoJsonSource with clustering options and verifies extracted
// data doesn't include source configuration like "cluster" or "clusterRadius"

// Why this matters:
// JavaScript's setData() expects ONLY GeoJSON data, not source configuration.
// If we accidentally include "cluster": true, it would corrupt the GeoJSON.
```

#### **Test: `JsonElement_Clone_Should_Work_For_Bulk_Transactions`**
```csharp
// What it does:
// Tests that JsonElement.Clone() preserves data for bulk transactions

// Why this is necessary:
// JsonDocument is disposed after the method returns, which would invalidate
// JsonElement references. Clone() creates a copy that survives disposal.
```

### 2. BulkTransactionTests.cs

These tests ensure bulk transactions work with the fix:

#### **Test: `BulkTransaction_Should_Support_SetSourceData`**
```csharp
// What it does:
// Simulates adding setSourceData to a bulk transaction and verifies
// the transaction structure is correct

// Why this matters:
// Bulk transactions batch multiple operations into one JS interop call.
// We need to ensure setSourceData can be part of these batches.
```

#### **Test: `BulkTransaction_Should_Mix_SetSourceData_With_Other_Operations`**
```csharp
// What it does:
// Tests that setSourceData can be mixed with addSource, removeSource, etc.

// Why this matters:
// Real applications might update multiple sources in one transaction.
// We verify operation order is preserved and data is intact.
```

### 3. SerializationApproachTests.cs

These tests compare different approaches to help choose the best one:

#### **Test: `Approach1_StringParse_Works` vs `Approach2_JsonNode_Works`**
```csharp
// What it does:
// Compares JsonDocument vs JsonNode approaches side-by-side

// Result:
// Both work identically. JsonNode is simpler and more efficient.
```

## Why Each Test Passes

### Core Principle

All tests pass because of one key insight:

**When you serialize a C# object with System.Text.Json, property-level `[JsonConverter]` attributes ARE applied during serialization.**

```csharp
// This applies the converter:
var json = JsonSerializer.Serialize(source);  // ← Converter runs here!

// Result: { "type": "geojson", "data": { ...properly serialized GeoJSON... } }

// Extracting "data" gives us the already-converted value:
var dataElement = jsonDoc.RootElement.GetProperty("data");  // ← Already converted!
```

### Test Categories

1. **Serialization Tests**: Verify the converter produces valid GeoJSON
   - Pass because: GeoJsonDataConverter correctly handles IFeature → JSON

2. **Extraction Tests**: Verify we extract only the data field
   - Pass because: JsonDocument/JsonNode property access works correctly

3. **JS Interop Tests**: Verify JsonElement/JsonNode serialize for JS
   - Pass because: Both types have proper JSON serialization support

4. **Bulk Transaction Tests**: Verify Clone() and transaction structure
   - Pass because: JsonElement.Clone() creates independent copies

5. **Edge Case Tests**: Empty collections, null properties, URLs
   - Pass because: GeoJsonDataConverter handles all these cases

## Recommendation: Simplify to JsonNode

The current implementation works, but we can simplify it:

### Before (Current - 4 lines, requires using):
```csharp
var json = System.Text.Json.JsonSerializer.Serialize(source);
using var jsonDoc = System.Text.Json.JsonDocument.Parse(json);
var dataElement = jsonDoc.RootElement.GetProperty("data");
// ... use dataElement
```

### After (Simpler - 2 lines, no using):
```csharp
var jsonNode = JsonSerializer.SerializeToNode(source);
var dataNode = jsonNode!["data"];
// ... use dataNode
```

**Benefits:**
- ✅ Fewer lines of code
- ✅ No string allocation and parsing
- ✅ No `using` statement needed
- ✅ More intuitive syntax
- ✅ Slightly better performance
- ✅ Same functionality

Both approaches are correct, but JsonNode is cleaner.
