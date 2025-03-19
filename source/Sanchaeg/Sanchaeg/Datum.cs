namespace Sanchaeg
{
    /// <summary>
    /// Datum types available
    /// </summary>
    public enum DatumType
    {
        // ReSharper disable InconsistentNaming
        WGS84,
        EPSG3857,
        GRS80,
        Airy1830,
        AiryModified,
        Intl1924,
        Bessel1841
        // ReSharper restore InconsistentNaming
    }

    /// <summary>
    /// Ellipsoid
    /// </summary>
    /// <param name="MajorAxis">Major axis of the ellipsoid ('a')</param>
    /// <param name="MinorAxis">Minor axis of the ellipsoid ('b')</param>
    /// <param name="Flattening">Flattening the ellipsoid ('f')</param>
    public record Ellipsoid(double MajorAxis, double MinorAxis, double Flattening);

    /// <summary>
    /// Geodetic datum
    /// </summary>
    public class Datum(DatumType type)
    {
        public Ellipsoid Ellipsoid => DatumMap[type];

        // transforms are not implemented!
        // more here: http://www.movable-type.co.uk/scripts/js/geodesy/latlon-ellipsoidal.js
        // and here: http://www.movable-type.co.uk/scripts/js/geodesy/latlon-vincenty.js

        private static readonly Dictionary<DatumType, Ellipsoid> DatumMap = new()
        {
            [DatumType.WGS84] = new Ellipsoid
            (
                MajorAxis: 6378137,
                MinorAxis: 6356752.31425,
                Flattening: 1/298.257223563
            ),
            [DatumType.EPSG3857] = new Ellipsoid
            (
                MajorAxis: 6378137,
                MinorAxis: 6378137,
                Flattening: 1 / 298.257223563
            ),
            [DatumType.GRS80] = new Ellipsoid
            (
                MajorAxis: 6378137,
                MinorAxis: 6356752.31414,
                Flattening: 1 /298.257222101
            ),
            [DatumType.Airy1830] = new Ellipsoid
            (
                MajorAxis: 6377563.396,
                MinorAxis: 6356256.909,
                Flattening: 1/299.3249646
            ),
            [DatumType.AiryModified] = new Ellipsoid
            (
                MajorAxis: 6377340.189,
                MinorAxis: 6356034.448,
                Flattening: 1/299.3249646
            ),
            [DatumType.Intl1924] = new Ellipsoid
            (
                MajorAxis: 6378388,
                MinorAxis: 6356911.946,
                Flattening: 1/297.0
            ),
            [DatumType.Bessel1841] = new Ellipsoid
            (
                MajorAxis: 6377397.155,
                MinorAxis: 6356078.963,
                Flattening: 1/299.152815351
            ),
        };
    }
}
