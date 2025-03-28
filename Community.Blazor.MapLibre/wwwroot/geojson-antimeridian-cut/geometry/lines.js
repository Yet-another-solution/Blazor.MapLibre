import {crossingPoints, antimeridianIntersect} from '../util.js';

/**
 * A LineString GeoJSON Object
 * @typedef {Object} LineString
 * @property {string} type
 * @property {number[][]} coordinates
 */

/**
 * A MultiLineString GeoJSON Object
 * @typedef {Object} MultiLineString
 * @property {string} type
 * @property {number[][][]} coordinates
 */

/**
 * Splits up an array of coordinates into an array of arrays of coordinates. The fundamental
 * function for breaking up (Multi)?LineStrings
 *
 * @param {number[][]} coordinates The list of coordinates to try to split
 * @returns {number[][][]}
 */
export const splitCoordinateArray = (coordinates) => {
    const crossings = crossingPoints(coordinates);
    if (crossings.length === 0) {
        return [coordinates];
    }

    const rtn = [];

    // Split up into segments on each side of meridian
    rtn.push(coordinates.slice(0, crossings[0] + 1));
    for (let i = 1; i < crossings.length; i += 1) {
        rtn.push(coordinates.slice(crossings[i - 1] + 1, crossings[i] + 1));
    }
    rtn.push(coordinates.slice(crossings[crossings.length - 1] + 1));

    // Add in the points on the meridian itself
    for (let i = 1; i < rtn.length; i += 1) {
        const left = rtn[i - 1];
        const lastLeft = left[left.length - 1];
        const right = rtn[i];
        const firstRight = right[0];

        const intersect = antimeridianIntersect(lastLeft, firstRight);

        left.push([180 * Math.sign(lastLeft[0]), intersect]);
        right.unshift([180 * Math.sign(firstRight[0]), intersect]);
    }

    return rtn;
};

/**
 * Examines the `coordinates` of `lineString` to see if it would cross the antimeridian, and if so,
 * transforms it into an equivalent `MultiLineString` broken up over the antimeridian.
 *
 * @param {LineString} lineString A `LineString` GeoJSON object to break up
 * @returns {LineString|MultiLineString}
 */
export const splitLineString = (lineString) => {
    const {coordinates, type: _type, ...rest} = lineString;

    const crossings = crossingPoints(coordinates);

    if (crossings.length === 0) {
        return {
            type: 'LineString',
            coordinates,
            ...rest,
        };
    }

    return {
        type: 'MultiLineString',
        coordinates: splitCoordinateArray(coordinates),
        ...rest,
    };
};

/**
 * Examines the `coordinates` of `multiLineString` to see if any of its components would cross the
 * antimeridian, and if so, transforms it into an equivalent `MultiLineString` broken up over the
 * antimeridian.
 *
 * @param {MultiLineString} multiLineString A `MultiLineString` GeoJSON Object to break up
 * @returns {MultiLineString}
 */
export const splitMultiLineString = (multiLineString) => {
    const {coordinates, type: _type, ...rest} = multiLineString;

    return {
        type: 'MultiLineString',
        coordinates: coordinates.map(splitCoordinateArray).flat(1),
        ...rest,
    };
};
