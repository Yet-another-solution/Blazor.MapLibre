import {splitLineString, splitMultiLineString} from './lines.js';
import {splitPolygon, splitMultiPolygon} from './polygons.js';

const splitGeometryObject = (geometryObject) => {
    switch (geometryObject.type) {
        case 'LineString':
            return splitLineString(geometryObject);
        case 'MultiLineString':
            return splitMultiLineString(geometryObject);
        case 'Polygon':
            return splitPolygon(geometryObject);
        case 'MultiPolygon':
            return splitMultiPolygon(geometryObject);
        case 'GeometryCollection':
            // eslint-disable-next-line no-use-before-define
            return splitGeometryCollection(geometryObject);
        case 'Point':
        case 'MultiPoint':
            // Do not change these types of GeoJSON Objects
            return geometryObject;
        default:
            throw new Error(`Must be some geometry object, is ${geometryObject.type}`);
    }
};

export const splitFeature = (feature) => {
    const {
        geometry,
        properties,
        type: _type,
        ...rest
    } = feature;

    const rtnGeometry = (geometry) ? splitGeometryObject(geometry) : null;

    return {
        type: 'Feature',
        geometry: rtnGeometry,
        properties,
        ...rest,
    };
};

export const splitFeatureCollection = (featureCollection) => {
    const {features, type: _type, ...rest} = featureCollection;

    return {
        type: 'FeatureCollection',
        features: features.map(splitFeature),
        ...rest,
    };
};

export const splitGeometryCollection = (geometryCollection) => {
    const {geometries, type: _type, ...rest} = geometryCollection;

    return {
        type: 'GeometryCollection',
        geometries: geometries.map(splitGeometryObject),
        ...rest,
    };
};
