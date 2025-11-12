export default {
    start: () => {

        // Create a base element to set the base URL for the Blazor app
        let baseElement = document.createElement('base')
        const isLocalhost = window.location.hostname === "localhost";
        baseElement.href = isLocalhost ? "/" : "/Blazor.MapLibre/";
        document.head.appendChild(baseElement);
        
        // Create a link element to import stylesheets
        let linkElement = document.createElement('link');
        linkElement.rel = 'stylesheet';
        linkElement.href = '_content/Community.Blazor.MapLibre/maplibre-5.12.0.min.css';
        document.head.appendChild(linkElement);
        
        // Create a script element to load the Blazor WebAssembly runtime
        let blazorScriptElement = document.createElement('script');
        blazorScriptElement.src = '_framework/blazor.webassembly.js';
        blazorScriptElement.autostart = 'true';
        document.body.appendChild(blazorScriptElement);
    },
}