const mapInstances = {};

const MapInterop = {
    initializeMap: (options, dotnetReference) => {
        const map = new maplibregl.Map(options);

        mapInstances[options.container] = map;

        map.on('load', function () {
            dotnetReference.invokeMethodAsync("OnLoadCallback")
        });
    },
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
    }
}
export { MapInterop };