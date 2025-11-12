# Community.Blazor.MapLibre.Tests

This test project contains unit tests for the Community.Blazor.MapLibre library, with a focus on JSON serialization/deserialization of GeoJSON features and geometries.

## Purpose

These tests verify that the library works correctly with .NET 8, 9, and 10, particularly ensuring that the fix for the `Polymorphism_PropertyConflictsWithMetadataPropertyName` error works as expected.

## Test Coverage

### GeometrySerializationTests
Tests for `IGeometry` interface and all geometry type implementations:
- PointGeometry
- LineGeometry
- PolygonGeometry
- MultiPointGeometry
- MultiLineGeometry
- MultiPolygonGeometry

Verifies:
- Serialization produces correct GeoJSON format
- Deserialization works with `$type` discriminator
- No polymorphism property conflicts occur
- GetBounds() functionality works correctly

### FeatureSerializationTests
Tests for `IFeature` interface and feature implementations:
- FeatureFeature (single feature)
- FeatureCollection (collection of features)

Verifies:
- Proper serialization of features with properties
- Nested geometry serialization within features
- Empty and populated collections
- Mixed geometry types in collections

### RealWorldScenarioTests
Integration tests simulating actual usage scenarios:
- SetSourceData pattern from the issue report
- GeoJSON source with feature collections
- Complex multi-geometry collections
- Round-trip serialization/deserialization
- Performance with larger datasets

## Running the Tests

### Using dotnet CLI

```bash
# Run all tests
dotnet test

# Run tests for a specific framework
dotnet test --framework net8.0
dotnet test --framework net9.0

# Run with verbose output
dotnet test --verbosity detailed

# Run a specific test class
dotnet test --filter "FullyQualifiedName~GeometrySerializationTests"
```

### Using Visual Studio
1. Open the solution in Visual Studio
2. Open Test Explorer (Test > Test Explorer)
3. Click "Run All" or select specific tests to run

### Using JetBrains Rider
1. Open the solution in Rider
2. Open Unit Tests window (View > Tool Windows > Unit Tests)
3. Right-click on the test project and select "Run Unit Tests"

## Test Frameworks

- **xUnit**: The testing framework used
- **FluentAssertions**: For more readable assertions
- **coverlet.collector**: For code coverage analysis

## Key Test Scenarios

### .NET 10 Polymorphism Fix
The most critical tests verify that changing `TypeDiscriminatorPropertyName` from `"type"` to `"$type"` resolves the .NET 10 serialization issue while maintaining GeoJSON compliance:

```csharp
[Fact]
public void Serialization_Should_Not_Throw_PropertyConflictsWithMetadataPropertyName_Exception()
{
    var point = new PointGeometry { Coordinates = new[] { -122.4194, 37.7749 } };
    Action act = () => JsonSerializer.Serialize<IGeometry>(point);
    act.Should().NotThrow<InvalidOperationException>();
}
```

### GeoJSON Compliance
Tests ensure that the serialized JSON maintains proper GeoJSON structure:

```json
{
  "$type": "Point",
  "type": "Point",
  "coordinates": [-122.4194, 37.7749]
}
```

Where:
- `$type` is used internally by .NET for polymorphic deserialization
- `type` is the GeoJSON standard property for geometry type

## Continuous Integration

These tests are designed to run in CI/CD pipelines to ensure compatibility across different .NET versions. Make sure to test against both .NET 8 and .NET 9 (and .NET 10 when available) to verify backwards compatibility.

## Contributing

When adding new features to the library, please include corresponding tests in this project. Follow the existing test patterns and naming conventions.
