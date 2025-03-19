// ReSharper disable InconsistentNaming

namespace Sanchaeg
{
    /// <summary>
    /// Spherical geography calculations
    /// </summary>
    public static class Spherical
    {
        /// <summary>
        /// Epsilon for floating point calculations
        /// </summary>
        private const double Epsilon = 0.00000005;

        /// <summary>
        /// Calculate the intersection of two point-bearing pairs
        /// </summary>
        /// <param name="a">first point</param>
        /// <param name="bearingA">first bearing</param>
        /// <param name="b">second point</param>
        /// <param name="bearingB">second bearing</param>
        /// <returns>intersection point</returns>
        public static SanchaegGeoCoordinate? Intersection(SanchaegGeoCoordinate a, double bearingA, SanchaegGeoCoordinate b, double bearingB)
        {
            // see http://williams.best.vwh.net/avform.htm#Intersection

            var φ1 = a.Latitude.ToRadians();
            var λ1 = a.Longitude.ToRadians();
            var φ2 = b.Latitude.ToRadians();
            var λ2 = b.Longitude.ToRadians();
            var θ13 = bearingA.ToRadians();
            var θ23 = bearingB.ToRadians();
            var Δφ = φ2 - φ1;
            var Δλ = λ2 - λ1;

            var δ12 = 2 * Math.Asin(Math.Sqrt(Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) + Math.Cos(φ1) * Math.Cos(φ2) * Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2)));
            if (Math.Abs(δ12) < Epsilon) return null;

            // initial/final bearings between points
            var θa = Math.Acos((Math.Sin(φ2) - Math.Sin(φ1) * Math.Cos(δ12)) / (Math.Sin(δ12) * Math.Cos(φ1)));
            if (double.IsNaN(θa)) θa = 0; // protect against rounding
            var θb = Math.Acos((Math.Sin(φ1) - Math.Sin(φ2) * Math.Cos(δ12)) / (Math.Sin(δ12) * Math.Cos(φ2)));

            var θ12 = Math.Sin(λ2 - λ1) > 0 ? θa : 2 * Math.PI - θa;
            var θ21 = Math.Sin(λ2 - λ1) > 0 ? 2 * Math.PI - θb : θb;

            var α1 = (θ13 - θ12 + Math.PI) % (2 * Math.PI) - Math.PI; // angle 2-1-3
            var α2 = (θ21 - θ23 + Math.PI) % (2 * Math.PI) - Math.PI; // angle 1-2-3

            if (Math.Abs(Math.Sin(α1)) < Epsilon && Math.Abs(Math.Sin(α2)) < Epsilon) return null; // infinite intersections
            if (Math.Sin(α1) * Math.Sin(α2) < 0) return null;      // ambiguous intersection

            //α1 = Math.abs(α1);
            //α2 = Math.abs(α2);
            // ... Ed Williams takes abs of α1/α2, but seems to break calculation?

            var α3 = Math.Acos(-Math.Cos(α1) * Math.Cos(α2) + Math.Sin(α1) * Math.Sin(α2) * Math.Cos(δ12));
            var δ13 = Math.Atan2(Math.Sin(δ12) * Math.Sin(α1) * Math.Sin(α2), Math.Cos(α2) + Math.Cos(α1) * Math.Cos(α3));
            var φ3 = Math.Asin(Math.Sin(φ1) * Math.Cos(δ13) + Math.Cos(φ1) * Math.Sin(δ13) * Math.Cos(θ13));
            var Δλ13 = Math.Atan2(Math.Sin(θ13) * Math.Sin(δ13) * Math.Cos(φ1), Math.Cos(δ13) - Math.Sin(φ1) * Math.Sin(φ3));
            var λ3 = λ1 + Δλ13;

            return new SanchaegGeoCoordinate
            {
                Latitude = φ3.ToDegrees(),
                Longitude = (λ3.ToDegrees() + 540) % 360 - 180
            };
        }
    }
}
