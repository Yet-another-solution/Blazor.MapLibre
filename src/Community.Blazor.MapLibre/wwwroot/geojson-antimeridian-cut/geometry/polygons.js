import {
    crossingPoints,
    antimeridianIntersect,
} from '../util.js';

/**
 * A Polygon GeoJSON object
 * @typedef {Object} Polygon
 * @property {string} type
 * @property {LinearRing[]} coordinates
 */

/**
 * A MultiPolygon GeoJSON object
 * @typedef {Object} MultiPolygon
 * @property {string} type
 * @property {LinearRing[][]} coordinates
 */

/**
 * An array of four or more coordinates that defines the boundary of a surface or a hole in a
 * surface. The first coordinate must be equivalent to the last.
 * @typedef {number[][]} LinearRing
 */

/**
 * @param {LinearRing} linearRing
 * @param {LinearRing[]} collector
 */
const splitLinearRing = (linearRing, collector) => {
    const crossings = crossingPoints(linearRing);
    if (crossings.length === 0) {
        collector.push([linearRing]);
        return;
    }

    // Get two highest-latitude intersection points
    const [startIntersect, endIntersect] = crossings
        .map(i => [
            antimeridianIntersect(linearRing[i], linearRing[i + 1]),
            i,
        ])
        .sort(([latA], [latB]) => latB - latA);

    // Traverse from first intersection and build 'left' polygon
    const leftRing = [];
    // Add the first point (the intersection point)
    let currentIndex = startIntersect[1] + 1;
    if (currentIndex === linearRing.length - 1) {
        currentIndex = 0;
    }
    let firstAfterIntersect = linearRing[currentIndex];

    leftRing.push([180 * Math.sign(firstAfterIntersect[0]), startIntersect[0]]);
    leftRing.push(linearRing[currentIndex]);
    while (currentIndex !== endIntersect[1]) {
        currentIndex += 1;
        if (currentIndex === linearRing.length - 1) { // End of ring, wrap around
            currentIndex = 0;
        }

        leftRing.push(linearRing[currentIndex]);
    }
    leftRing.push([180 * Math.sign(firstAfterIntersect[0]), endIntersect[0]]);

    // Traverse from second intersection and build 'right' polygon;
    const rightRing = [];
    currentIndex = endIntersect[1] + 1;
    if (currentIndex === linearRing.length - 1) {
        currentIndex = 0;
    }
    firstAfterIntersect = linearRing[currentIndex];

    rightRing.push([180 * Math.sign(firstAfterIntersect[0]), endIntersect[0]]);
    rightRing.push(linearRing[currentIndex]);
    while (currentIndex !== startIntersect[1]) {
        currentIndex += 1;
        if (currentIndex === linearRing.length - 1) {
            currentIndex = 0;
        }

        rightRing.push(linearRing[currentIndex]);
    }
    rightRing.push([180 * Math.sign(firstAfterIntersect[0]), startIntersect[0]]);

    // Duplicate first value at end to create a ring
    leftRing.push(leftRing[0]);
    rightRing.push(rightRing[0]);

    splitLinearRing(leftRing, collector);
    splitLinearRing(rightRing, collector);
};

/**
 * Splits a polygon's linear rings. The first ring in the list is treated as the outer loop. The
 * remaining rings, if they exist, are treated as holes. If the main ring is split, all holes are
 * added to all resulting polygons. If a hole is split, the resulting rings are also applied to all
 * top-level polygons.
 *
 * @param {LinearRing[]} rings A list of rings to spli
 * @returns {LinearRing[][]}
 */
const splitPolygonRingList = (rings) => {
    const [mainRing, ...holes] = rings;

    const rtn = [];

    // Split the main ring & push results into rtn value
    splitLinearRing(mainRing, rtn);

    // Split each hole and apply the cut holes to each resulting ring
    if (holes) {
        const cutHoles = [];
        holes.forEach(hole => splitLinearRing(hole, cutHoles));
        rtn.forEach(ring => cutHoles.flat().forEach(hole => ring.push(hole)));
    }

    return rtn;
};

/**
 * @param {Polygon} polygon
 * @returns {(MultiPolygon|Polygon)}
 */
export const splitPolygon = (polygon) => {
    const {coordinates: linearRings, type: _type, ...rest} = polygon;

    const cutPolygon = splitPolygonRingList(linearRings);
    return (cutPolygon.length > 1)
        ? {
            type: 'MultiPolygon',
            coordinates: cutPolygon,
            ...rest,
        }
        : {
            type: 'Polygon',
            coordinates: cutPolygon.flat(),
            ...rest,
        };
};

/**
 * @param {MultiPolygon} multiPolygon
 * @returns {MultiPolygon}
 */
export const splitMultiPolygon = (multiPolygon) => {
    const {coordinates: polygons, type: _type, ...rest} = multiPolygon;

    const rtn = polygons.map(polygon => splitPolygonRingList(polygon)).flat();

    return {
        type: 'MultiPolygon',
        coordinates: rtn,
        ...rest,
    };
};
