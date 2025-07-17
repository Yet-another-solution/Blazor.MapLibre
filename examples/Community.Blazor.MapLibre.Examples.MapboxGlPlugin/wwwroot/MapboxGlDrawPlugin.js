let mapObject = {};
let drawObject = {};
let pluginDotnetReference = {};

export function initialize(map, dotnetReference) {
    console.log("Plugin initialized");
    mapObject = map;
    pluginDotnetReference = dotnetReference;
}

/**
 * Adds a Mapbox Draw control to the map instance.
 *
 * @param {Object} drawOptions - Options for configuring the Mapbox Draw control.
 * @return {void} No return value.
 */
export function addControl(drawOptions) {
    MapboxDraw.constants.classes.CANVAS  = 'maplibregl-canvas';
    MapboxDraw.constants.classes.CONTROL_BASE  = 'maplibregl-ctrl';
    MapboxDraw.constants.classes.CONTROL_PREFIX = 'maplibregl-ctrl-';
    MapboxDraw.constants.classes.CONTROL_GROUP = 'maplibregl-ctrl-group';
    MapboxDraw.constants.classes.ATTRIBUTION = 'maplibregl-ctrl-attrib';

    drawObject = new MapboxDraw(drawOptions)
    mapObject.addControl(drawObject);

    mapObject.on('draw.create', updateArea);
    mapObject.on('draw.delete', updateArea);
    mapObject.on('draw.update', updateArea);

    function updateArea(e) {
        const data = drawObject.getAll();

        // Current JavaScript call
        pluginDotnetReference.invokeMethodAsync("OnDrawUpdateCallback", data, e.type);
        console.log(data);
    }
}

/**
 * Adds a feature to the draw instance associated with the specified container.
 *
 * @param {Object} feature - The feature object to be added to the draw instance.
 * @return {void} No return value.
 */
export function addFeature(feature) {
    drawObject.add(feature);
}