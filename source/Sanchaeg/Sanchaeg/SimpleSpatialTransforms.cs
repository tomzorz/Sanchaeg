namespace Sanchaeg
{
    /// <summary>
    /// Simple spatial transforms
    /// </summary>
    public static class SimpleSpatialTransforms
    {
        /// <summary>
        /// Default datum used for the calculations
        /// </summary>
        private static readonly Datum Datum = new(DatumType.EPSG3857);

        // methodology from below
        // https://alastaira.wordpress.com/2011/01/23/the-google-maps-bing-maps-spherical-mercator-projection/

        // testing EPSG3857
        // https://epsg.io/transform#s_srs=3857&t_srs=4326&x=1916112.1383870&y=6135193.5181356

        /// <summary>
        /// Convert a GeoCoordinate to a Coordinate
        /// </summary>
        /// <param name="geoCoordinate">geoCoordinate</param>
        /// <param name="customDatum">optional custom datum</param>
        /// <returns>Coordinate equivalent</returns>
        public static SanchaegCoordinate ToCoordinate(this SanchaegGeoCoordinate geoCoordinate, Datum? customDatum = null)
        {
            var ellipsoid = customDatum?.Ellipsoid ?? Datum.Ellipsoid;
            var x = geoCoordinate.Longitude * (ellipsoid.MajorAxis * Math.PI) / 180;
            var y = Math.Log(Math.Tan((90 + geoCoordinate.Latitude) * Math.PI / 360)) / (Math.PI / 180);
            y = y * (ellipsoid.MinorAxis * Math.PI) / 180;
            return new SanchaegCoordinate(x, y);
        }

        /// <summary>
        /// Convert a Coordinate to a GeoCoordinate
        /// </summary>
        /// <param name="coordinate">coordinate</param>
        /// <param name="customDatum">optional custom datum</param>
        /// <returns>GeoCoordinate equivalent</returns>
        public static SanchaegGeoCoordinate ToGeoCoordinate(this SanchaegCoordinate coordinate, Datum? customDatum = null)
        {
            var ellipsoid = customDatum?.Ellipsoid ?? Datum.Ellipsoid;
            var lon = (coordinate.X / (ellipsoid.MajorAxis * Math.PI)) * 180;
            var lat = (coordinate.Y / (ellipsoid.MinorAxis * Math.PI)) * 180;
            lat = 180 / Math.PI * (2 * Math.Atan(Math.Exp(lat * Math.PI / 180)) - Math.PI / 2);
            return new SanchaegGeoCoordinate(lat, lon);
        }
    }
}
