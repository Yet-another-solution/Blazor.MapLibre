import splitGeoJSON from './geojson-antimeridian-cut/cut.js'

const mapInstances = {};
const optionsInstances = {};

/**
 * Cuts the GeoJSON source at the antimeridian if the option is enabled.
 *
 * @param {string} container - The identifier for the map container instance.
 * @param {Object} data - The GeoJSON data you wish to apply to the source
 * @returns {Object} - The GeoJSON data with the antimeridian cut if the option is enabled
 */
function cutAntiMeridian(container, data) {
    if (optionsInstances[container]?.cutAtAntimeridian !== true) {
        return data;
    }

    return splitGeoJSON(data);
}

/**
 * Initializes a MapLibre map instance with the given options and connects it to a .NET reference for interop functionality.
 *
 * @param {Object} options - Configuration options for initializing the MapLibre map instance.
 * @param {Object} dotnetReference - A .NET instance reference for invoking interop methods.
 */
export function initializeMap(options, dotnetReference) {
    const map = new maplibregl.Map(options);
    
    optionsInstances[options.container] = options;
    mapInstances[options.container] = map;

    map.on('load', function () {
        dotnetReference.invokeMethodAsync("OnLoadCallback")
    });
}

/**
 * Attaches an event listener to a specified map instance.
 *
 * @param {string} container - The identifier for the specific map instance.
 * @param {string} eventType - The type of event to listen for (e.g., "click", "zoom").
 * @param {object} dotnetReference - A reference to a .NET object used for invoking asynchronous methods.
 * @param {string | string[]} layerIds - Optional layer to pass when adding the event listener. If not provided, the event is added without a layer.
 */
export function on(container, eventType, dotnetReference, layerIds) {
    if (layerIds === undefined || layerIds === null) {
        mapInstances[container].on(eventType, function (e) {
            e.target = null; // Remove map to prevent circular references.
            const result = JSON.stringify(e);
            dotnetReference.invokeMethodAsync('Invoke', result)
        })
    } else {
        mapInstances[container].on(eventType, layerIds, function (e) {
            e.target = null; // Remove map to prevent circular references.
            const result = JSON.stringify(e);
            dotnetReference.invokeMethodAsync('Invoke', result)
        })
    }
}
/**
 * Adds a specified control to the given map container.
 *
 * @param {string} container - The identifier of the map container.
 * @param {string} controlType - The type of control to add. Supported types include:
 *                               "AttributionControl", "FullscreenControl", "GeolocateControl",
 *                               "GlobeControl", "LogoControl", "NavigationControl", "ScaleControl",
 *                               and "TerrainControl".
 * @param {string} position - position on the map to which the control will be added. Valid values are 'top-left', 'top-right', 'bottom-left', and 'bottom-right'. Defaults to 'top-right'.
 *
 * @throws {Error} Logs a warning if the specified control type is not supported.
 */
export function addControl(container, controlType, position) {
    const map = mapInstances[container];
    const controlsMap = {
        AttributionControl: maplibregl.AttributionControl,
        FullscreenControl: maplibregl.FullscreenControl,
        GeolocateControl: maplibregl.GeolocateControl,
        GlobeControl: maplibregl.GlobeControl,
        LogoControl: maplibregl.LogoControl,
        NavigationControl: maplibregl.NavigationControl,
        ScaleControl: maplibregl.ScaleControl,
        TerrainControl: maplibregl.TerrainControl
    };

    const ControlClass = controlsMap[controlType];
    if (ControlClass) {
        const control = new ControlClass(position);
        map.addControl(control);
    } else {
        console.warn(`Control type '${controlType}' is not supported.`);
    }
}

/**
 * Asynchronously adds an image to a map instance for the specified container.
 *
 * @param {string} container - The identifier of the map container.
 * @param {string} id - The unique identifier for the image to add.
 * @param {string} url - The URL from which the image should be loaded.
 * @param {Object} [options] - Optional configuration settings for the image.
 * @returns {Promise<void>} Resolves when the image is successfully added or if the image already exists.
 */
export async function addImage(container, id, url, options) {
    const map = mapInstances[container];
    if (map.hasImage(id)) return;
    const resourceResponse = await mapInstances[container].loadImage(url);
    if (options === undefined || options === null) {
        map.addImage(id, resourceResponse.data);
    } else {
        map.addImage(id, resourceResponse.data, options);
    }
}

/**
 * Adds a layer to the specified map container.
 *
 * @function
 * @param {string} container - The identifier for the map container where the layer will be added.
 * @param {Object} layer - The layer object to be added to the map.
 * @param {string} [beforeId] - The ID of an existing layer before which the new layer will be added. Optional.
 */
export function addLayer(container, layer, beforeId) {
    mapInstances[container].addLayer(layer, beforeId);
}

/**
 * Adds a new source to the specified map container instance.
 *
 * @param {string} container - The identifier for the map container instance.
 * @param {string} id - The unique identifier for the new source.
 * @param {Object} source - The source configuration object to be added.
 */
export function addSource(container, id, source) {
    if (source.type === 'geojson') {
        const data = cutAntiMeridian(container, source.data);
        source.data = data;
    }

    mapInstances[container].addSource(id, source);
}

/**
 * Updates the data of a specific GeoJSON source
 * 
 * @param {string} container - The identifier for the map container instance.
 * @param {string} id - The unique identifier for the source you wish to update.
 * @param {Object} data - The GeoJSON data you wish to apply to the source
 */
export function setSourceData(container, id, data) {
    data = cutAntiMeridian(container, data);
    const source = mapInstances[container].getSource(id);
    if (source === undefined) {
        throw new Error(`Could not find source with id ${id}`);
    }
    source.setData(data);
}

/**
 * Adds a sprite to the specified map container.
 *
 * @function
 * @param {string} container - The identifier for the map container.
 * @param {string} id - The unique identifier for the sprite.
 * @param {string} url - The URL of the sprite image.
 * @param {Object} [options] - Optional parameters for the sprite, such as pixel ratio.
 */
export function addSprite(container, id, url, options) {
    const map = mapInstances[container];
    if (options === undefined || options === null) {
        map.addSprite(id, url);
    } else {
        map.addSprite(id, url, options);
    }
}

/**
 * Checks if all map tiles are loaded for the given container.
 *
 * @param {string} container - The identifier of the map container to check.
 * @returns {boolean} Returns true if all tiles are loaded, otherwise false.
 */
export function areTilesLoaded(container) {
    return mapInstances[container].areTilesLoaded();
}

/**
 * Calculates the camera options based on the provided longitude, latitude, altitude, bearing, pitch, and roll.
 *
 * @param {string} container - The container identifier for the map instance.
 * @param {Array<number>} cameraLngLat - The longitude and latitude of the camera position as an array [longitude, latitude].
 * @param {number} cameraAltitude - The altitude of the camera in meters.
 * @param {number} bearing - The bearing of the camera in degrees.
 * @param {number} pitch - The pitch of the camera in degrees.
 * @param {number} roll - The roll of the camera in degrees.
 * @returns {Object} The calculated camera options.
 */
export function calculateCameraOptionsFromCameraLngLatAltRotation(container, cameraLngLat, cameraAltitude, bearing, pitch, roll) {
    return mapInstances[container].calculateCameraOptionsFromCameraLngLatAltRotation(cameraLngLat, cameraAltitude, bearing, pitch, roll);
}

/**
 * Calculates camera options for transitioning from one location to another.
 *
 * @param {string} container - The identifier of the map container.
 * @param {Array<number>} from - The starting coordinates of the transition [latitude, longitude].
 * @param {number} altitudeFrom - The altitude at the starting location.
 * @param {Array<number>} to - The destination coordinates of the transition [latitude, longitude].
 * @param {number} altitudeTo - The altitude at the destination location.
 * @returns {Object} The calculated camera options for the transition.
 */
export function calculateCameraOptionsFromTo(container, from, altitudeFrom, to, altitudeTo) {
    return mapInstances[container].calculateCameraOptionsFromTo(from, altitudeFrom, to, altitudeTo);
}

/**
 * Calculates and returns the camera options needed to fit the provided bounds within the specified container.
 *
 * @param {string} container - The identifier for the map container.
 * @param {Object} bounds - The geographical bounds to be displayed within the container.
 * @param {Object} [options] - Optional settings for adjusting the camera view, such as padding or max zoom.
 * @returns {Object} The camera options to fit the bounds within the specified container.
 */
export function cameraForBounds(container, bounds, options) {
    return mapInstances[container].cameraForBounds(bounds, options);
}

/**
 * Smoothly animates the map viewport to the specified location and zoom level.
 *
 * @param {string} container - The identifier of the map container.
 * @param {Object} options - Configuration options for easing to the target state, such as center, zoom, bearing, and pitch.
 * @param {Object} [eventData] - Optional additional metadata for the event triggered by the easing action.
 */
export function easeTo(container, options, eventData) {
    mapInstances[container].easeTo(options, eventData);
}

/**
 * Adjusts the map view within the specified container to fit the given geographical bounds.
 *
 * @param {string} container - The identifier for the map container that needs to fit the bounds.
 * @param {Object} bounds - The geographical bounds to fit within the map view.
 * @param {Object} [options] - Optional settings for fitting the bounds, such as padding or animation.
 * @param {Object} [eventData] - Optional event-related data to be associated with this operation.
 */
export function fitBounds(container, bounds, options, eventData) {
    mapInstances[container].fitBounds([
        [bounds._sw.lng, bounds._sw.lat], // Southwest corner
        [bounds._ne.lng, bounds._ne.lat]  // Northeast corner
    ], options, eventData);
}

/**
 * Adjusts the map view to fit the given screen coordinates.
 *
 * @function fitScreenCoordinates
 * @param {string} container - The identifier for the map container instance.
 * @param {Array<number>} p0 - The first screen coordinate as [x, y].
 * @param {Array<number>} p1 - The second screen coordinate as [x, y].
 * @param {number} bearing - The map's bearing in degrees to apply during the fit.
 * @param {Object} [options] - Optional configuration parameters for fitting the coordinates.
 * @param {Object} [eventData] - Optional event data to be dispatched with the fit operation.
 */
export function fitScreenCoordinates(container, p0, p1, bearing, options, eventData) {
    mapInstances[container].fitScreenCoordinates(p0, p1, bearing, options, eventData);
}

/**
 * Animates the map view to a specified center and zoom level with a smooth transition effect.
 *
 * @function
 * @param {string} container - The identifier for the map container instance to apply the flyTo action.
 * @param {Object} options - The flyTo options containing the target coordinates, zoom level, and optional padding.
 * @param {Object} eventData - Optional additional event data associated with the flyTo action.
 */
export function flyTo(container, options, eventData) {
    mapInstances[container].flyTo(options, eventData);
}

/**
 * Retrieves the current bearing (direction) of the map associated with the specified container.
 * The bearing typically refers to the compass direction, in degrees, that the map is rotated to.
 *
 * @param {string} container - The identifier of the container associated with the map instance.
 * @returns {number} The bearing of the map in degrees.
 */
export function getBearing(container) {
    return mapInstances[container].getBearing();
}

/**
 * Retrieves the geographical boundaries of a map instance associated with the specified container.
 *
 * @param {string} container - The identifier of the container associated with the map instance.
 * @returns {Object} An object representing the geographical boundaries of the map instance.
 */
export function getBounds(container) {
    return mapInstances[container].getBounds();
}

/**
 * Retrieves the camera target elevation for the specified map container.
 *
 * This function checks if the `getCameraTargetElevation` method is available
 * for the given map instance. If it is, the method is invoked to obtain the
 * camera target elevation. If the method is not supported, a warning message
 * is logged to the console, and `null` is returned.
 *
 * @param {string} container - The identifier of the map container for which the
 * camera target elevation is to be retrieved.
 * @returns {number|null} The camera target elevation if supported, or `null`
 * if the method is not available for the map instance.
 */
export function getCameraTargetElevation(container) {
    if (mapInstances[container].getCameraTargetElevation) {
        return mapInstances[container].getCameraTargetElevation();
    } else {
        console.warn("getCameraTargetElevation is not supported for this map instance.");
        return null;
    }
}

/**
 * Retrieves the canvas element associated with a specific container.
 *
 * @param {string} container - The identifier for the container whose canvas is to be retrieved.
 * @returns {HTMLCanvasElement} The canvas element associated with the specified container.
 */
export function getCanvas(container) {
    return mapInstances[container].getCanvas();
}

/**
 * Retrieves the canvas container associated with the given container.
 *
 * @param {string} container - The identifier for the container whose canvas container is to be retrieved.
 * @returns {Object} The canvas container associated with the specified container.
 */
export function getCanvasContainer(container) {
    return mapInstances[container].getCanvasContainer();
}

/**
 * Retrieves the center coordinates of the map instance associated with the specified container.
 *
 * @param {string} container - The identifier of the container associated with the map instance.
 * @returns {Object} An object representing the center coordinates of the map.
 */
export function getCenter(container) {
    return mapInstances[container].getCenter();
}

/**
 * Retrieves the center point of the map clamped to the ground for the specified container.
 * The center point corresponds to the geographical coordinates at the center of the map view.
 *
 * @param {string} container - The identifier for the map container instance.
 * @returns {Object} An object representing the center coordinates clamped to the ground.
 */
export function getCenterClampedToGround(container) {
    return mapInstances[container].getCenterClampedToGround();
}

/**
 * Gets the center elevation of the map instance associated with the provided container.
 *
 * @function
 * @param {string} container - The identifier for the container whose map instance's center elevation is to be retrieved.
 * @returns {number} The elevation value at the center of the map.
 */
export function getCenterElevation(container) {
    return mapInstances[container].getCenterElevation();
}

/**
 * Retrieves the container instance corresponding to the given container identifier.
 *
 * @param {string} container - The identifier for the desired container.
 * @returns {*} The container instance associated with the specified container identifier.
 */
export function etContainer(container) {
    return mapInstances[container].getContainer();
}

/**
 * Retrieves the state of a specific feature within a given container.
 *
 * @param {string} container - The identifier for the container where the feature is located.
 * @param {Object} feature - The feature object whose state needs to be retrieved.
 * @returns {Object} The current state of the specified feature.
 */
export function getFeatureState(container, feature) {
    return mapInstances[container].getFeatureState(feature);
}

/**
 * Retrieves the filter configuration for a specified layer in the given map container.
 *
 * @param {string} container - The identifier of the map container instance.
 * @param {string} layerId - The identifier of the layer whose filter is being retrieved.
 * @returns {Array|Object|null} The filter configuration applied to the specified layer, or null if no filter exists.
 */
export function getFilter(container, layerId) {
    return mapInstances[container].getFilter(layerId);
}

/**
 * Retrieves the glyphs associated with the specified container.
 *
 * @param {string} container - The identifier of the container to fetch glyphs for.
 * @returns {Array} An array of glyph objects associated with the container.
 */
export function getGlyphs(container) {
    return mapInstances[container].getGlyphs();
}

/**
 * Retrieves an image from the specified container using its identifier.
 *
 * @param {string} container - The name of the container from which to retrieve the image.
 * @param {string} id - The unique identifier of the image to retrieve.
 * @returns {*} The image associated with the given identifier in the specified container.
 */
export function getImage(container, id) {
    return mapInstances[container].getImage(id);
}

/**
 * Retrieves a specific layer from a map instance associated with the given container.
 *
 * @param {string} container - The identifier of the container holding the map instance.
 * @param {string} id - The unique identifier of the layer to retrieve.
 * @returns {Object|undefined} The layer object if found, or undefined if the layer does not exist.
 */
export function getLayer(container, id) {
    return mapInstances[container].getLayer(id);
}

/**
 * Retrieves the order of layers within a specific map container.
 *
 * @param {string} container - The identifier for the map container whose layer order is to be retrieved.
 * @returns {Array} An array representing the order of layers in the specified container.
 */
export function getLayersOrder(container) {
    return mapInstances[container].getLayersOrder();
}

/**
 * Retrieves the value of a specified layout property for a given layer in a map container.
 *
 * @function
 * @param {string} container - The identifier of the map container instance.
 * @param {string} layerId - The unique identifier of the layer whose property is being accessed.
 * @param {string} name - The name of the layout property to retrieve.
 * @returns {*} The value of the requested layout property, as defined within the specified layer.
 */
export function getLayoutProperty(container, layerId, name) {
    return mapInstances[container].getLayoutProperty(layerId, name);
}

/**
 * Retrieves the light object of the map style.
 * @param {string} container - The identifier of the map container.
 * @returns {Object} The light specification of the map style.
 */
export function getLight(container) {
    return mapInstances[container].getLight();
}

/**
 * Retrieves the maximum geographical bounds the map is constrained to.
 * @param {string} container - The map container identifier.
 * @returns {Object|null} The max bounds (LngLatBounds) or null if not set.
 */
export function getMaxBounds(container) {
    return mapInstances[container].getMaxBounds();
}

/**
 * Retrieves the map's maximum allowable pitch.
 * @param {string} container - The identifier of the map container.
 * @returns {number} Maximum allowable pitch in degrees.
 */
export function getMaxPitch(container) {
    return mapInstances[container].getMaxPitch();
}

/**
 * Retrieves the map's maximum allowable zoom level.
 * @param {string} container - The identifier of the map container.
 * @returns {number} Maximum allowable zoom level.
 */
export function getMaxZoom(container) {
    return mapInstances[container].getMaxZoom();
}

/**
 * Retrieves the map's minimum allowable pitch.
 * @param {string} container - The identifier of the map container.
 * @returns {number} Minimum allowable pitch in degrees.
 */
export function getMinPitch(container) {
    return mapInstances[container].getMinPitch();
}

/**
 * Retrieves the map's minimum allowable zoom level.
 * @param {string} container - The identifier of the map container.
 * @returns {number} Minimum allowable zoom level.
 */
export function getMinZoom(container) {
    return mapInstances[container].getMinZoom();
}

/**
 * Retrieves the current padding applied around the map viewport.
 * @param {string} container - The identifier of the map container.
 * @returns {Object} Padding options applied to the map.
 */
export function getPadding(container) {
    return mapInstances[container].getPadding();
}

/**
 * Retrieves the value of a paint property of a specified layer.
 * @param {string} container - The identifier of the map container.
 * @param {string} layerId - The ID of the layer to get the paint property from.
 * @param {string} name - The name of the paint property.
 * @returns {*} The value of the specified paint property.
 */
export function getPaintProperty(container, layerId, name) {
    return mapInstances[container].getPaintProperty(layerId, name);
}

/**
 * Retrieves the current pitch (tilt) of the map in degrees.
 * @param {string} container - The identifier of the map container.
 * @returns {number} The current pitch of the map.
 */
export function getPitch(container) {
    return mapInstances[container].getPitch();
}

/**
 * Retrieves the map's pixel ratio.
 * Note: The actual applied pixel ratio may be lower than specified due to max canvas size restrictions.
 * @param {string} container - The identifier of the map container.
 * @returns {number} The pixel ratio of the map.
 */
export function getPixelRatio(container) {
    return mapInstances[container].getPixelRatio();
}

/**
 * Retrieves the projection specification of the map.
 * @param {string} container - The identifier of the map container.
 * @returns {Object} The projection specification of the map.
 */
export function getProjection(container) {
    return mapInstances[container].getProjection();
}

/**
 * Retrieves the state of `renderWorldCopies`.
 * @param {string} container - The identifier of the map container.
 * @returns {boolean} True if multiple world copies are rendered, false otherwise.
 */
export function getRenderWorldCopies(container) {
    return mapInstances[container].getRenderWorldCopies();
}

/**
 * Retrieves the current roll angle of the map in degrees.
 * @param {string} container - The identifier of the map container.
 * @returns {number} The roll angle of the map.
 */
export function getRoll(container) {
    return mapInstances[container].getRoll();
}

/**
 * Retrieves the sky properties of the map style.
 * @param {string} container - The identifier of the map container.
 * @returns {Object} The sky properties of the map.
 */
export function getSky(container) {
    return mapInstances[container].getSky();
}

/**
 * Retrieves a source by its ID from the map's style.
 * @param {string} container - The identifier of the map container.
 * @param {string} id - The ID of the source to retrieve.
 * @returns {Object|undefined} The source object if found, or undefined.
 */
export function getSource(container, id) {
    return mapInstances[container].getSource(id);
}

/**
 * Retrieves the style's sprite as a list of objects.
 * @param {string} container - The identifier of the map container.
 * @returns {Array} The list of sprite objects for the style.
 */
export function getSprite(container) {
    return mapInstances[container].getSprite();
}

/**
 * Retrieves the map's style specification.
 * @param {string} container - The identifier of the map container.
 * @returns {Object} The style specification object of the map.
 */
export function getStyle(container) {
    return mapInstances[container].getStyle();
}

/**
 * Retrieves the terrain options if terrain is loaded.
 * @param {string} container - The identifier of the map container.
 * @returns {Object|undefined} The terrain specification object or undefined if not loaded.
 */
export function getTerrain(container) {
    return mapInstances[container].getTerrain();
}

/**
 * Retrieves the map's current vertical field of view in degrees.
 * @param {string} container - The identifier of the map container.
 * @returns {number} The vertical field of view of the map.
 */
export function getVerticalFieldOfView(container) {
    return mapInstances[container].getVerticalFieldOfView();
}

/**
 * Retrieves the map's current zoom level.
 * @param {string} container - The identifier of the map container.
 * @returns {number} The current zoom level of the map.
 */
export function getZoom(container) {
    return mapInstances[container].getZoom();
}

/**
 * Checks if a control exists on the map.
 * @param {string} container - The identifier of the map container.
 * @param {IControl} control - The control to check.
 * @returns {boolean} True if the control exists on the map.
 */
export function hasControl(container, control) {
    return mapInstances[container].hasControl(control);
}

/**
 * Checks whether an image with the given ID exists in the style.
 * @param {string} container - The identifier of the map container.
 * @param {string} id - ID of the image.
 * @returns {boolean} True if the image exists.
 */
export function hasImage(container, id) {
    return mapInstances[container].hasImage(id);
}

/**
 * Returns true if the map is moving.
 * @param {string} container - The identifier of the map container.
 * @returns {boolean} True if the map is moving.
 */
export function sMoving(container) {
    return mapInstances[container].isMoving();
}

/**
 * Returns true if the map is rotating.
 * @param {string} container - The identifier of the map container.
 * @returns {boolean} True if the map is rotating.
 */
export function isRotating(container) {
    return mapInstances[container].isRotating();
}

/**
 * Returns true if the specified source is loaded.
 * @param {string} container - The identifier of the map container.
 * @param {string} id - The source ID.
 * @returns {boolean} True if the source is loaded.
 */
export function isSourceLoaded(container, id) {
    return mapInstances[container].isSourceLoaded(id);
}

/**
 * Returns true if the map's style is fully loaded.
 * @param {string} container - The identifier of the map container.
 * @returns {boolean} True if the style is loaded.
 */
export function isStyleLoaded(container) {
    return mapInstances[container].isStyleLoaded();
}

/**
 * Returns true if the map is zooming.
 * @param {string} container - The identifier of the map container.
 * @returns {boolean} True if the map is zooming.
 */
export function isZooming(container) {
    return mapInstances[container].isZooming();
}

/**
 * Changes any combination of center, zoom, bearing, pitch, or roll without animation.
 * @param {string} container - The map container.
 * @param {object} options - JumpToOptions for updating the map view.
 * @param {any} eventData - Optional event data.
 */
export function jumpTo(container, options, eventData) {
    mapInstances[container].jumpTo(options, eventData);
}

/**
 * Returns true if there is at least one registered listener for a given event type.
 * @param {string} container - The map container.
 * @param {string} type - The event type.
 * @returns {boolean} True if a listener exists for the event type.
 */
export function listens(container, type) {
    return mapInstances[container].listens(type);
}

/**
 * Lists all image IDs available in the map's style.
 * @param {string} container - The map container.
 * @returns {string[]} A list of all image IDs.
 */
export function istImages(container) {
    return mapInstances[container].listImages();
}

/**
 * Checks if the map is fully loaded.
 * @param {string} container - The map container.
 * @returns {boolean} True if the map is fully loaded.
 */
export function loaded(container) {
    return mapInstances[container].loaded();
}

/**
 * Loads an image from an external URL.
 * @param {string} container - The map container.
 * @param {string} url - The URL for the image.
 * @returns {Promise<*>} A promise resolving when the image is loaded.
 */
export async function loadImage(container, url) {
    return await mapInstances[container].loadImage(url);
}

/**
 * Moves a layer to a different z-position.
 * @param {string} container - The map container.
 * @param {string} id - The ID of the layer to move.
 * @param {string} beforeId - The ID of the target layer to place the moved layer before.
 */
export function moveLayer(container, id, beforeId) {
    mapInstances[container].moveLayer(id, beforeId);
}

/**
 * Pans the map by the specified offset.
 * @param {string} container - The map container.
 * @param {Array} offset - The pan offset.
 * @param {object} options - Pan options.
 * @param {any} eventData - Optional event data.
 */
export function panBy(container, offset, options, eventData) {
    mapInstances[container].panBy(offset, options, eventData);
}

/**
 * Pans the map to the specified location.
 * @param {string} container - The map container.
 * @param {Array<number>} lngLat - The target longitude and latitude.
 * @param {object} options - Pan animation options.
 * @param {any} eventData - Optional event data.
 */
export function panTo(container, lngLat, options, eventData) {
    mapInstances[container].panTo(lngLat, options, eventData);
}

/**
 * Projects geographical coordinates to pixel coordinates.
 * @param {string} container - The map container.
 * @param {Array<number>} lngLat - Longitude and latitude to project.
 * @returns {Object} The projected point.
 */
export function project(container, lngLat) {
    return mapInstances[container].project(lngLat);
}

/**
 * Queries rendered features.
 * @param {string} container - The map container.
 * @param {object} query - The query options or geometry.
 * @param {object} options - Rendered features query options.
 * @returns {Array} Query results.
 */
export function queryRenderedFeatures(container, query, options) {
    return mapInstances[container].queryRenderedFeatures(query, options);
}

/**
 * Queries features from a source.
 * @param {string} container - The map container.
 * @param {string} sourceId - The source ID.
 * @param {object} parameters - Query parameters.
 * @returns {Array} Query results.
 */
export function querySourceFeatures(container, sourceId, parameters) {
    return mapInstances[container].querySourceFeatures(sourceId, parameters);
}

/**
 * Queries terrain elevation at a given location.
 * @param {string} container - The map container.
 * @param {Array<number>} lngLat - Longitude and latitude to query.
 * @returns {number} Elevation in meters.
 */
export function queryTerrainElevation(container, lngLat) {
    return mapInstances[container].queryTerrainElevation(lngLat);
}

/**
 * Forces a redraw of the map.
 * @param {string} container - The map container.
 */
export function redraw(container) {
    mapInstances[container].redraw();
}

/**
 * Cleans up internal resources associated with the map.
 * @param {string} container - The map container.
 */
export function remove(container) {
    mapInstances[container].remove();
    delete mapInstances[container];
}

/**
 * Removes a control from the map.
 * @param {string} container - The map container.
 * @param {IControl} control - The control to remove.
 */
export function removeControl(container, control) {
    mapInstances[container].removeControl(control);
}

/**
 * Removes feature states from the map.
 * @param {string} container - The map container.
 * @param {Object} target - The feature or source to remove states.
 * @param {string} key - Optional key of the state to remove.
 */
export function removeFeatureState(container, target, key) {
    mapInstances[container].removeFeatureState(target, key);
}

/**
 * Removes an image from the map.
 * @param {string} container - The map container.
 * @param {string} id - The ID of the image to remove.
 */
export function removeImage(container, id) {
    mapInstances[container].removeImage(id);
}

/**
 * Removes a layer by its ID.
 * @param {string} container - The map container.
 * @param {string} id - The ID of the layer to remove.
 */
export function removeLayer(container, id) {
    mapInstances[container].removeLayer(id);
}

/**
 * Removes a source from the map's style.
 * @param {string} container - The map container.
 * @param {string} id - The ID of the source to remove.
 */
export function removeSource(container, id) {
    mapInstances[container].removeSource(id);
}

/**
 * Removes the sprite from the map's style.
 * @param {string} container - The map container.
 * @param {string} id - The ID of the sprite to remove.
 */
export function removeSprite(container, id) {
    mapInstances[container].removeSprite(id);
}

/**
 * Rotates the map so that north is up.
 * @param {string} container - The map container.
 * @param {object} [options] - Animation options.
 * @param {any} [eventData] - Additional event data.
 */
export function resetNorth(container, options, eventData) {
    mapInstances[container].resetNorth(options, eventData);
}

/**
 * Resets the map's north and pitch angles with an animated transition.
 * @param {string} container - The map container.
 * @param {object} [options] - Animation options.
 * @param {any} [eventData] - Additional event data.
 */
export function resetNorthPitch(container, options, eventData) {
    mapInstances[container].resetNorthPitch(options, eventData);
}

/**
 * Resizes the map according to its container's dimensions.
 * @param {string} container - The map container.
 * @param {any} [eventData] - Additional event data.
 * @param {boolean} [constrainTransform=true] - Transform constraint flag.
 */
export function resize(container, eventData, constrainTransform = true) {
    mapInstances[container].resize(eventData, constrainTransform);
}

/**
 * Rotates the map to the specified bearing.
 * @param {string} container - The map container.
 * @param {number} bearing - The target bearing.
 * @param {object} [options] - Animation options.
 * @param {any} [eventData] - Additional event data.
 */
export function rotateTo(container, bearing, options, eventData) {
    mapInstances[container].rotateTo(bearing, options, eventData);
}

/**
 * Sets the map's bearing (rotation).
 * @param {string} container - The map container.
 * @param {number} bearing - The target bearing.
 * @param {any} [eventData] - Additional event data.
 */
export function setBearing(container, bearing, eventData) {
    mapInstances[container].setBearing(bearing, eventData);
}

/**
 * Sets the map's geographical center point.
 * @param {string} container - The map container.
 * @param {Array<number>} center - The center coordinates [lng, lat].
 * @param {any} [eventData] - Additional event data.
 */
export function setCenter(container, center, eventData) {
    mapInstances[container].setCenter(center, eventData);
}

/**
 * Sets the map's centerClampedToGround value.
 * @param {string} container - The map container.
 * @param {boolean} centerClampedToGround - Clamped to ground flag.
 */
export function setCenterClampedToGround(container, centerClampedToGround) {
    mapInstances[container].setCenterClampedToGround(centerClampedToGround);
}

/**
 * Sets the elevation of the map's center point.
 * @param {string} container - The map container.
 * @param {number} elevation - The target elevation in meters.
 * @param {any} [eventData] - Additional event data.
 */
export function setCenterElevation(container, elevation, eventData) {
    mapInstances[container].setCenterElevation(elevation, eventData);
}

/**
 * Sets the event parent to bubble events to.
 * @param {string} container - The map container.
 * @param {Evented} parent - The parent Evented instance.
 * @param {any} [data] - Additional data.
 */
export function setEventedParent(container, parent, data) {
    mapInstances[container].setEventedParent(parent, data);
}

/**
 * Sets the state of a specific feature.
 * @param {string} container - The map container.
 * @param {Object} feature - Feature identifier object.
 * @param {Object} state - State to apply.
 */
export function setFeatureState(container, feature, state) {
    mapInstances[container].setFeatureState(feature, state);
}

/**
 * Sets a filter for a specified layer.
 * @param {string} container - The map container.
 * @param {string} layerId - The layer ID.
 * @param {object} [filter] - Filter to apply.
 * @param {object} [options] - Filter options.
 */
export function setFilter(container, layerId, filter, options) {
    mapInstances[container].setFilter(layerId, filter, options);
}

/**
 * Sets the map's glyph source URL.
 * @param {string} container - The map container.
 * @param {string} glyphsUrl - The glyph URL.
 * @param {object} options - Options object.
 */
export function setGlyphs(container, glyphsUrl, options) {
    mapInstances[container].setGlyphs(glyphsUrl, options);
}

/**
 * Updates the map's style.
 * @param {string} container - The map container.
 * @param {string | object} style - The new style URL or JSON.
 * @param {object} [options] - Style options.
 */
export function setStyle(container, style, options) {
    mapInstances[container].setStyle(style, options);
}

/**
 * Loads a 3D terrain mesh using a "raster-dem" source.
 * @param {string} container - The map container.
 * @param {object} options - Terrain specification options.
 */
export function setTerrain(container, options) {
    mapInstances[container].setTerrain(options);
}

/**
 * Updates the requestManager's transform request with a new function.
 * @param {string} container - The map container.
 * @param {Function} transformRequest - Callback to modify requests.
 */
export function setTransformRequest(container, transformRequest) {
    mapInstances[container].setTransformRequest(transformRequest);
}

/**
 * Sets the map's vertical field of view in degrees.
 * @param {string} container - The map container.
 * @param {number} fov - The target vertical field of view (0-180 degrees).
 * @param {any} [eventData] - Additional event data.
 */
export function setVerticalFieldOfView(container, fov, eventData) {
    mapInstances[container].setVerticalFieldOfView(fov, eventData);
}

/**
 * Sets the map's zoom level.
 * @param {string} container - The map container.
 * @param {number} zoom - The desired zoom level (0-20).
 * @param {any} [eventData] - Additional event data.
 */
export function setZoom(container, zoom, eventData) {
    mapInstances[container].setZoom(zoom, eventData);
}

/**
 * Snaps the map so that north (0° bearing) is up, if the current bearing is close enough.
 * @param {string} container - The map container.
 * @param {object} [options] - Animation options.
 * @param {any} [eventData] - Additional event data.
 */
export function snapToNorth(container, options, eventData) {
    mapInstances[container].snapToNorth(options, eventData);
}

/**
 * Stops any animated transition currently underway.
 * @param {string} container - The map container.
 */
export function stop(container) {
    mapInstances[container].stop();
}

/**
 * Triggers the rendering of a single frame.
 * Use this method with custom layers to force rendering updates.
 * @param {string} container - The map container.
 */
export function triggerRepaint(container) {
    mapInstances[container].triggerRepaint();
}

/**
 * Converts pixel coordinates (x, y) to geographical coordinates (longitude, latitude).
 * @param {string} container - The map container.
 * @param {Array<number>} point - The pixel coordinates [x, y].
 * @returns {Array<number>} Geographical coordinates [lng, lat].
 */
export function unproject(container, point) {
    return mapInstances[container].unproject(point);
}

/**
 * Updates an existing image in the map's sprite.
 * @param {string} container - The map container.
 * @param {string} id - The image ID.
 * @param {ImageBitmap|HTMLImageElement|ImageData|Object} image - The new image data to update.
 */
export function updateImage(container, id, image) {
    mapInstances[container].updateImage(id, image);
}

/**
 * Increases the map's zoom level by 1.
 * @param {string} container - The map container.
 * @param {object} [options] - Animation options object.
 * @param {any} [eventData] - Additional event data.
 */
export function zoomIn(container, options, eventData) {
    mapInstances[container].zoomIn(options, eventData);
}

/**
 * Decreases the map's zoom level by 1.
 * @param {string} container - The map container.
 * @param {object} [options] - Animation options object.
 * @param {any} [eventData] - Additional event data.
 */
export function zoomOut(container, options, eventData) {
    mapInstances[container].zoomOut(options, eventData);
}

/**
 * Zooms the map to a specific zoom level with animation.
 * @param {string} container - The map container.
 * @param {number} zoom - The target zoom level.
 * @param {object} [options] - Options for animation like duration, offset, etc.
 * @param {any} [eventData] - Additional event data.
 */
export function zoomTo(container, zoom, options, eventData) {
    mapInstances[container].zoomTo(zoom, options, eventData);
}

/**
 * Perform all applied bulk transactions.
 * The only purpose of bulk transaction send multiple transactions in one message, reducing the roundtrip time.
 * Each action in the transaction is performed in the order they are received.
 * @param {string} container - The map container.
 * @param {object} data - Options for animation like duration, offset, etc.
 */
export async function executeTransaction(container, data) {
    for (const d of data) {
        switch (d.event) {
            case "addControl":
                addControl(container, d.data[0], d.data[1]);
                break;
            case "addImage":
                await addImage(container, d.data[0], d.data[1], d.data[2]);
                break;
            case "addLayer":
                addLayer(container, d.data[0], d.data[1]);
                break;
            case "addSource":
                addSource(container, d.data[0], d.data[1]);
                break;
            case "addSprite":
                addSprite(container, d.data[0], d.data[1], d.data[2]);
                break;
            case "removeControl":
                removeControl(container, d.data[0]);
                break;
            case "removeFeatureState":
                removeFeatureState(container, d.data[0], d.data[1]);
                break;
            case "removeImage":
                removeImage(container, d.data[0]);
                break;
            case "removeLayer":
                removeLayer(container, d.data[0]);
                break;
            case "removeSource":
                removeSource(container, d.data[0]);
                break;
            case "removeSprite":
                removeSprite(container, d.data[0]);
                break;
            default:
                console.warn(`Unknown transaction event: ${d.event}`);
                throw new Error(`Unknown transaction event: ${d.event}`);
        }
    }
}