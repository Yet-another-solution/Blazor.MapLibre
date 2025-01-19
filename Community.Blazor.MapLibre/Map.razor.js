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
    }
}
export { MapInterop };