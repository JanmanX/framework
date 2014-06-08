﻿
using System;
using Mercraft.Core.Utilities;

namespace Mercraft.Core
{
    /// <summary>
    /// Represents bounding box
    /// See details: http://stackoverflow.com/questions/238260/how-to-calculate-the-bounding-box-for-a-given-lat-lng-location
    /// </summary>
    public class BoundingBox
    {
        public GeoCoordinate MinPoint { get; set; }
        public GeoCoordinate MaxPoint { get; set; }
        
        // Semi-axes of WGS-84 geoidal reference
        private const double WGS84_a = 6378137.0; // Major semiaxis [m]
        private const double WGS84_b = 6356752.3; // Minor semiaxis [m]

        public BoundingBox(GeoCoordinate minPoint, GeoCoordinate maxPoint)
        {
            MinPoint = minPoint;
            MaxPoint = maxPoint;
        }

        public bool Contains(double latitude, double longitude)
        {
            return (MaxPoint.Latitude > latitude && latitude >= MinPoint.Latitude) &&
                 (MaxPoint.Longitude > longitude && longitude >= MinPoint.Longitude);
        }

        public bool Contains(GeoCoordinate point)
        {
            return Contains(point.Latitude, point.Longitude);
        }

        #region Operations

        /// <summary>
        /// Adds point to bounding boxes together yielding as result the smallest box that surrounds both.
        /// </summary>
        public static BoundingBox operator +(BoundingBox a, GeoCoordinate b)
        {
            GeoCoordinate minPoint = new GeoCoordinate(
                a.MinPoint.Latitude < b.Latitude ? a.MinPoint.Latitude : b.Latitude,
                a.MinPoint.Longitude < b.Longitude ? a.MinPoint.Longitude : b.Longitude);

            GeoCoordinate maxPoint = new GeoCoordinate(
                a.MaxPoint.Latitude > b.Latitude ? a.MaxPoint.Latitude : b.Latitude,
                a.MaxPoint.Longitude > b.Longitude ? a.MaxPoint.Longitude : b.Longitude);

            return new BoundingBox(minPoint, maxPoint);
        }

        public static BoundingBox operator +(BoundingBox a, BoundingBox b)
        {
            var minLat = a.MinPoint.Latitude < b.MinPoint.Latitude ? a.MinPoint.Latitude : b.MinPoint.Latitude;
            var minLon = a.MinPoint.Longitude < b.MinPoint.Longitude ? a.MinPoint.Longitude : b.MinPoint.Longitude;

            var maxLat = a.MaxPoint.Latitude > b.MaxPoint.Latitude ? a.MaxPoint.Latitude : b.MaxPoint.Latitude;
            var maxLon = a.MaxPoint.Longitude > b.MaxPoint.Longitude ? a.MaxPoint.Longitude : b.MaxPoint.Longitude;

            return new BoundingBox(new GeoCoordinate(minLat, minLon), new GeoCoordinate(maxLat, maxLon));
        }

        #endregion

        # region Creation

        /// <summary>
        /// Creates bounding box
        /// </summary>
        /// <param name="point">Center</param>
        /// <param name="halfSideInM">Half length of the bounding box</param>
        public static BoundingBox CreateBoundingBox(GeoCoordinate point, double halfSideInM)
        {
            // Bounding box surrounding the point at given coordinates,
            // assuming local approximation of Earth surface as a sphere
            // of radius given by WGS84
            var lat = MathUtility.Deg2Rad(point.Latitude);
            var lon = MathUtility.Deg2Rad(point.Longitude);

            // Radius of Earth at given latitude
            var radius = WGS84EarthRadius(lat);
            // Radius of the parallel at given latitude
            var pradius = radius* Math.Cos(lat);

            var latMin = lat - halfSideInM / radius;
            var latMax = lat + halfSideInM / radius;
            var lonMin = lon - halfSideInM / pradius;
            var lonMax = lon + halfSideInM / pradius;

            return new BoundingBox(
                new GeoCoordinate(MathUtility.Rad2Deg(latMin), MathUtility.Rad2Deg(lonMin)),
                new GeoCoordinate(MathUtility.Rad2Deg(latMax), MathUtility.Rad2Deg(lonMax)));
        }


        /// <summary>
        /// Earth radius at a given latitude, according to the WGS-84 ellipsoid [m]
        /// </summary>
        private static double WGS84EarthRadius(double lat)
        {
            // http://en.wikipedia.org/wiki/Earth_radius
            var an = WGS84_a*WGS84_a*Math.Cos(lat);
            var bn = WGS84_b*WGS84_b*Math.Sin(lat);
            var ad = WGS84_a*Math.Cos(lat);
            var bd = WGS84_b*Math.Sin(lat);
            return Math.Sqrt((an*an + bn*bn)/(ad*ad + bd*bd));
        }

        #endregion
    }
}
