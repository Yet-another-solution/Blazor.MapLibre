/**
 * @example
 * bringLonWithinBounds(1) === 1;
 * bringLonWithinBounds(181) === 1;
 * bringLonWithinBounds(-365) === -5;
 *
 * @param {number} lon The longitude to reduce
 * @returns {number} A longitude equivalent to `lon` between [-180, 180]
 */
export const bringLonWithinBounds = (lon) => {
    let rtn = lon;

    while (rtn > 180) {
        rtn -= 360;
    }

    while (rtn < -180) {
        rtn += 360;
    }

    return rtn;
};

/**
 * Checks if the line drawn between two points would cross the antimeridian.
 * @param {number[]} a The first point to check
 * @param {number[]} b The second point to check
 *
 * @returns {boolean} `true` if the points straddle the antimeridian, `false` otherwise
 */
export const straddlesAntimeridian = ([lonARaw], [lonBRaw]) => {
    // If they are more than 360 degrees apart, they must cross the antimeridian.
    // Only applies to points with coords > 180 or < -180
    if (lonARaw - lonBRaw >= 360 || lonARaw - lonBRaw <= -360) {
        return true;
    }

    const lonA = bringLonWithinBounds(lonARaw);
    const lonB = bringLonWithinBounds(lonBRaw);

    if (Math.sign(lonA) === Math.sign(lonB)) {
        return false;
    }

    return ((lonA > 0)
        ? (lonB < -(180 - lonA))
        : (lonA < -(180 - lonB))
    );
};

/**
 * Finds the points at which a list of coordinates crosses the antimeridian
 *
 * @param {number[][]} points The points to check
 *
 * @returns {number[]} The indicies of the `points` that occur immediately before a crossing
 */
export const crossingPoints = (points) => {
    const rtn = [];

    for (let i = 1; i < points.length; i += 1) {
        const a = points[i - 1];
        const b = points[i];

        if (straddlesAntimeridian(a, b)) {
            rtn.push(i - 1);
        }
    }

    return rtn;
};

const makeLonPositive = lon => ((lon < 0) ? makeLonPositive(lon + 360) : lon);

/**
 * Calculates the latitude at which a line drawn between two coordinates would intersect
 * the antimeridian. Assumes that:
 * * The line would cross the antimeridian
 * * -180 < lon{A,B} < 180
 * * -90 < lat{A,B} < 90
 *
 * @param {number[]} a The first point to check, [lon, lat]
 * @param {number[]} b The second point to check, [lon, lat]
 *
 * @returns {number} The Antimeridian Intersect of `a` and `b`
 */
export const antimeridianIntersect = ([lonA, latA], [lonB, latB]) => {
    const lonANorm = makeLonPositive(lonA);
    const lonBNorm = makeLonPositive(lonB);

    const slope = (latB - latA) / (lonBNorm - lonANorm);

    return slope * (180 - lonANorm) + latA;
};
