const mapInstances = {};

const MapInterop = {
    /**
     * Initializes a MapLibre map instance with the given options and connects it to a .NET reference for interop functionality.
     *
     * @param {Object} options - Configuration options for initializing the MapLibre map instance.
     * @param {Object} dotnetReference - A .NET instance reference for invoking interop methods.
     */
    initializeMap: (options, dotnetReference) => {
        const map = new maplibregl.Map(options);

        mapInstances[options.container] = map;

        map.on('load', function () {
            dotnetReference.invokeMethodAsync("OnLoadCallback")
        });
    },
    /**
     * Attaches an event listener to a specified map instance.
     *
     * @param {string} container - The identifier for the specific map instance.
     * @param {string} eventType - The type of event to listen for (e.g., "click", "zoom").
     * @param {object} dotnetReference - A reference to a .NET object used for invoking asynchronous methods.
     * @param {object} [args] - Optional argument to pass when adding the event listener. If not provided, the event is added without additional arguments.
     */
    on: (container, eventType, dotnetReference, args) => {
        if (args === undefined || args === null) {
            mapInstances[container].on(eventType, function (e) {
                e.target = null; // Remove map to prevent circular references.
                const result = JSON.stringify(e);
                dotnetReference.invokeMethodAsync('Invoke', result)
            })
        }
        else {
           mapInstances[container].on(eventType, args, function (e) {
                e.target = null; // Remove map to prevent circular references.
                const result = JSON.stringify(e);
                dotnetReference.invokeMethodAsync('Invoke', result)
            })
        }
    },
    /**
     * Adds a specified control to the given map container.
     *
     * @param {string} container - The identifier of the map container.
     * @param {string} controlType - The type of control to add. Supported types include:
     *                               "AttributionControl", "FullscreenControl", "GeolocateControl",
     *                               "GlobeControl", "LogoControl", "NavigationControl", "ScaleControl",
     *                               and "TerrainControl".
     * @param {Object} options - Configuration options for the control instance being added.
     *
     * @throws {Error} Logs a warning if the specified control type is not supported.
     */
    addControl: (container, controlType, options) => {
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
            const control = new ControlClass(options);
            map.addControl(control);
        } else {
            console.warn(`Control type '${controlType}' is not supported.`);
        }
    },
    /**
     * Asynchronously adds an image to a map instance for the specified container.
     *
     * @param {string} container - The identifier of the map container.
     * @param {string} id - The unique identifier for the image to add.
     * @param {string} url - The URL from which the image should be loaded.
     * @param {Object} [options] - Optional configuration settings for the image.
     * @returns {Promise<void>} Resolves when the image is successfully added or if the image already exists.
     */
    addImage: async (container, id, url, options) => {
        const map = mapInstances[container];
        if (map.hasImage(id)) return;
        const resourceResponse = await mapInstances[container].loadImage(url);
        if(options === undefined || options === null){
            map.addImage(id, resourceResponse.data);
        } else {
            map.addImage(id, resourceResponse.data, options);
        }
    },
    /**
     * Adds a layer to the specified map container.
     *
     * @function
     * @param {string} container - The identifier for the map container where the layer will be added.
     * @param {Object} layer - The layer object to be added to the map.
     * @param {string} [beforeId] - The ID of an existing layer before which the new layer will be added. Optional.
     */
    addLayer: (container, layer, beforeId) => {
        mapInstances[container].addLayer(layer, beforeId);
    },
    /**
     * Adds a new source to the specified map container instance.
     *
     * @param {string} container - The identifier for the map container instance.
     * @param {string} id - The unique identifier for the new source.
     * @param {Object} source - The source configuration object to be added.
     */
    addSource: (container, id, source) => {
        mapInstances[container].addSource(id, source);
    },
    /**
     * Adds a sprite to the specified map container.
     *
     * @function
     * @param {string} container - The identifier for the map container.
     * @param {string} id - The unique identifier for the sprite.
     * @param {string} url - The URL of the sprite image.
     * @param {Object} [options] - Optional parameters for the sprite, such as pixel ratio.
     */
    addSprite: (container, id, url, options) => {
        const map = mapInstances[container];
        if(options === undefined || options === null){
            map.addSprite(id, url);
        } else {
            map.addSprite(id, url, options);
        }
    },
    /**
     * Checks if all map tiles are loaded for the given container.
     *
     * @param {string} container - The identifier of the map container to check.
     * @returns {boolean} Returns true if all tiles are loaded, otherwise false.
     */
    areTilesLoaded: (container) => {
        return mapInstances[container].areTilesLoaded();
    },
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
    calculateCameraOptionsFromCameraLngLatAltRotation: (container, cameraLngLat, cameraAltitude, bearing, pitch, roll) => {
        return mapInstances[container].calculateCameraOptionsFromCameraLngLatAltRotation(cameraLngLat, cameraAltitude, bearing, pitch, roll);
    },
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
    calculateCameraOptionsFromTo: (container, from, altitudeFrom, to, altitudeTo) => {
        return mapInstances[container].calculateCameraOptionsFromTo(from, altitudeFrom, to, altitudeTo);
    },
    /**
     * Calculates and returns the camera options needed to fit the provided bounds within the specified container.
     *
     * @param {string} container - The identifier for the map container.
     * @param {Object} bounds - The geographical bounds to be displayed within the container.
     * @param {Object} [options] - Optional settings for adjusting the camera view, such as padding or max zoom.
     * @returns {Object} The camera options to fit the bounds within the specified container.
     */
    cameraForBounds: (container, bounds, options) => {
        return mapInstances[container].cameraForBounds(bounds, options);
    },
    /**
     * Smoothly animates the map viewport to the specified location and zoom level.
     *
     * @param {string} container - The identifier of the map container.
     * @param {Object} options - Configuration options for easing to the target state, such as center, zoom, bearing, and pitch.
     * @param {Object} [eventData] - Optional additional metadata for the event triggered by the easing action.
     */
    easeTo: (container, options, eventData) => {
        mapInstances[container].easeTo(options, eventData);
    },
    /**
     * Adjusts the map view within the specified container to fit the given geographical bounds.
     *
     * @param {string} container - The identifier for the map container that needs to fit the bounds.
     * @param {Object} bounds - The geographical bounds to fit within the map view.
     * @param {Object} [options] - Optional settings for fitting the bounds, such as padding or animation.
     * @param {Object} [eventData] - Optional event-related data to be associated with this operation.
     */
    fitBounds: (container, bounds, options, eventData) => {
        mapInstances[container].fitBounds(bounds, options, eventData);
    },
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
    fitScreenCoordinates: (container, p0, p1, bearing, options, eventData) => {
        mapInstances[container].fitScreenCoordinates(p0, p1, bearing, options, eventData);
    },
    /**
     * Animates the map view to a specified center and zoom level with a smooth transition effect.
     *
     * @function
     * @param {string} container - The identifier for the map container instance to apply the flyTo action.
     * @param {Object} options - The flyTo options containing the target coordinates, zoom level, and optional padding.
     * @param {Object} eventData - Optional additional event data associated with the flyTo action.
     */
    flyTo: (container, options, eventData) => {
        mapInstances[container].flyTo(options, eventData);
    },
    /**
     * Retrieves the current bearing (direction) of the map associated with the specified container.
     * The bearing typically refers to the compass direction, in degrees, that the map is rotated to.
     *
     * @param {string} container - The identifier of the container associated with the map instance.
     * @returns {number} The bearing of the map in degrees.
     */
    getBearing: (container) => {
        return mapInstances[container].getBearing();
    },
    /**
     * Retrieves the geographical boundaries of a map instance associated with the specified container.
     *
     * @param {string} container - The identifier of the container associated with the map instance.
     * @returns {Object} An object representing the geographical boundaries of the map instance.
     */
    getBounds: (container) => {
        return mapInstances[container].getBounds();
    },
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
    getCameraTargetElevation: (container) => {
        if (mapInstances[container].getCameraTargetElevation) {
            return mapInstances[container].getCameraTargetElevation();
        } else {
            console.warn("getCameraTargetElevation is not supported for this map instance.");
            return null;
        }
    },
    /**
     * Retrieves the canvas element associated with a specific container.
     *
     * @param {string} container - The identifier for the container whose canvas is to be retrieved.
     * @returns {HTMLCanvasElement} The canvas element associated with the specified container.
     */
    getCanvas: (container) => {
        return mapInstances[container].getCanvas();
    },
    /**
     * Retrieves the canvas container associated with the given container.
     *
     * @param {string} container - The identifier for the container whose canvas container is to be retrieved.
     * @returns {Object} The canvas container associated with the specified container.
     */
    getCanvasContainer: (container) => {
        return mapInstances[container].getCanvasContainer();
    },
    /**
     * Retrieves the center coordinates of the map instance associated with the specified container.
     *
     * @param {string} container - The identifier of the container associated with the map instance.
     * @returns {Object} An object representing the center coordinates of the map.
     */
    getCenter: (container) => {
        return mapInstances[container].getCenter();
    },
}
export { MapInterop };