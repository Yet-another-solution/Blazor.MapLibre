import { splitLineString, splitMultiLineString } from './geometry/lines.js';
import { splitPolygon, splitMultiPolygon } from './geometry/polygons.js';
import { splitFeature, splitFeatureCollection, splitGeometryCollection } from './geometry/features.js';
import './util.js';

/**
 * Recursively up a GeoJSON Object (or its children, if applicable)
 * @param {Object} object The GeoJSON Object to split up
 * @param {string} object.type
 * @param {number[]} [object.coordinates]
 * @param {Object[]} [object.features]
 * @param {Object[]} [object.geometries]
 *
 * @returns {Object} The processed `object`
 */
const splitGeoJSON = (object) => {
    if (!object.type) {
        throw new Error('Object is not a valid GeoJSON Object, must have \'type\' property');
    }

    if (typeof object.type !== 'string') {
        throw new Error(`Property 'type' of GeoJSON Object must be 'string', is ${object.type}`);
    }

    switch (object.type) {
        case 'LineString':
            return splitLineString(object);
        case 'MultiLineString':
            return splitMultiLineString(object);
        case 'Polygon':
            return splitPolygon(object);
        case 'MultiPolygon':
            return splitMultiPolygon(object);
        case 'Feature':
            return splitFeature(object);
        case 'FeatureCollection':
            return splitFeatureCollection(object);
        case 'GeometryCollection':
            return splitGeometryCollection(object);
        case 'Point':
        case 'MultiPoint':
            // Do not change these types of GeoJSON Objects
            return object;
        default:
            throw new Error(`Invalid 'type' of GeoJSON object: ${object.type}`);
    }
};

export default splitGeoJSON;
