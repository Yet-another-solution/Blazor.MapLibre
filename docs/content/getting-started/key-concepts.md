# Key concepts

Blazor.MapLibre enables .NET developers to create sophisticated mapping experiences without leaving the comfort of C# and the Blazor ecosystem.

Blazor.MapLibre is a comprehensive Blazor component library that provides a C# wrapper around the core [MapLibre GL JS](https://maplibre.org/maplibre-gl-js/docs/) JavaScript library. It enables .NET developers to integrate powerful, interactive mapping capabilities into their Blazor applications using familiar C# syntax and component patterns.

## MapLibre GL JS wrapper

At its foundation, Blazor.MapLibre wraps the powerful MapLibre GL JS library, which is:

- An open-source JavaScript library for interactive, customizable maps
- Capable of rendering vector tiles and custom styling
- Highly performant with WebGL rendering
- A community-maintained fork of Mapbox GL JS

The wrapper provides a seamless bridge between .NET code and the underlying JavaScript functionality, abstracting away the complexities of JavaScript interop.

## Blazor component

Blazor.MapLibre follows Blazor's component-based architecture and makes the `MapLibre` component available to app authors.

- Encapsulates map functionality in reusable Blazor components
- Provides strong typing and IntelliSense support for all MapLibre features
- Integrates with Blazor's rendering and lifecycle management
- Enables declarative map configuration through component parameters

## Plugin system

The library features an extensible plugin system.

- Allows for modular extension of core functionality
- Provides a standardized way to extend maps with specialized features
- Maintains a clean separation between core library and optional extensions

See further guidance to [Create a plugin](./create-a-plugin.md).
