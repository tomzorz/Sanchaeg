// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable TooWideLocalVariableScope

namespace Sanchaeg
{
    /// <summary>
    /// Vincenty formulae for distance and bearing calculations
    /// </summary>
    public static class Vincenty
    {
        /// <summary>
        /// The geodetic datum used for the calculations
        /// </summary>
        private static readonly Datum Datum = new(DatumType.WGS84);

        public static double CalculateDistanceBetween(this SanchaegGeoCoordinate origin, SanchaegGeoCoordinate other, Datum? customDatum = null)
        {
            try
            {
                return CalculateInverse(origin, other, customDatum?.Ellipsoid ?? Datum.Ellipsoid).Item1;
            }
            catch
            {
                return 0.0;
            }
        }

        public static SanchaegGeoCoordinate CalculateDestinationPoint(this SanchaegGeoCoordinate origin, double distance, double bearing, Datum? customDatum = null)
        {
            try
            {
                return CalculateDirect(origin, distance, bearing, customDatum?.Ellipsoid ?? Datum.Ellipsoid).Item1;
            }
            catch
            {
                return new SanchaegGeoCoordinate();
            }
        }

        public static double CalculateActualBearing(this SanchaegGeoCoordinate origin, double distance, double bearing, Datum? customDatum = null)
        {
            try
            {
                return CalculateDirect(origin, distance, bearing, customDatum?.Ellipsoid ?? Datum.Ellipsoid).Item2;
            }
            catch
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Calculate distance using Vincenty's formulae
        /// </summary>
        /// <param name="origin">origin point</param>
        /// <param name="target">target point</param>
        /// <param name="datum">datum</param>
        /// <returns>distance, initial bearing, final bearing</returns>
        private static (double, double, double) CalculateInverse(SanchaegGeoCoordinate origin, SanchaegGeoCoordinate target, Ellipsoid datum)
        {
            var p1 = origin;
            var p2 = target;
            var φ1 = p1.Latitude.ToRadians();
            var λ1 = p1.Longitude.ToRadians();
            var φ2 = p2.Latitude.ToRadians();
            var λ2 = p2.Longitude.ToRadians();

            var a = datum.MajorAxis;
            var b = datum.MinorAxis;
            var f = datum.Flattening;

            var L = λ2 - λ1;
            var tanU1 = (1 - f)*Math.Tan(φ1);
            var cosU1 = 1/Math.Sqrt((1 + tanU1*tanU1));
            var sinU1 = tanU1 * cosU1;
            var tanU2 = (1 - f)*Math.Tan(φ2);
            var cosU2 = 1/Math.Sqrt((1 + tanU2*tanU2));
            var sinU2 = tanU2 * cosU2;

            double sinλ, cosλ, sinSqσ, sinσ, cosσ, σ, sinα, cosSqα, cos2σM, C;

            var λ = L;
            double  λʹ;
            var iterations = 0;
            do
            {
                sinλ = Math.Sin(λ);
                cosλ = Math.Cos(λ);
                sinSqσ = (cosU2 * sinλ) * (cosU2 * sinλ) + (cosU1 * sinU2 - sinU1 * cosU2 * cosλ) * (cosU1 * sinU2 - sinU1 * cosU2 * cosλ);
                sinσ = Math.Sqrt(sinSqσ);
                if (Math.Abs(sinσ) < 0.00001) return (0.0, 0.0, 0.0);  // co-incident points
                cosσ = sinU1 * sinU2 + cosU1 * cosU2 * cosλ;
                σ = Math.Atan2(sinσ, cosσ);
                sinα = cosU1 * cosU2 * sinλ / sinσ;
                cosSqα = 1 - sinα * sinα;
                cos2σM = cosσ - 2 * sinU1 * sinU2 / cosSqα;
                if (double.IsNaN(cos2σM)) cos2σM = 0;  // equatorial line: cosSqα=0 (§6)
                C = f / 16.0 * cosSqα * (4 + f * (4 - 3 * cosSqα));
                λʹ = λ;
                λ = L + (1 - C) * f * sinα * (σ + C * sinσ * (cos2σM + C * cosσ * (-1 + 2 * cos2σM * cos2σM)));
            } while (Math.Abs(λ - λʹ) > 1e-12 && ++iterations < 200);
            if (iterations >= 200) throw new Exception("Formula failed to converge");

            var uSq = cosSqα * (a * a - b * b) / (b * b);
            var A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
            var B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));
            var Δσ = B * sinσ * (cos2σM + B / 4 * (cosσ * (-1 + 2 * cos2σM * cos2σM) -
                B / 6 * cos2σM * (-3 + 4 * sinσ * sinσ) * (-3 + 4 * cos2σM * cos2σM)));

            var s = b * A * (σ - Δσ);

            var α1 = Math.Atan2(cosU2 * sinλ, cosU1 * sinU2 - sinU1 * cosU2 * cosλ);
            var α2 = Math.Atan2(cosU1 * sinλ, -sinU1 * cosU2 + cosU1 * sinU2 * cosλ);

            α1 = (α1 + 2 * Math.PI) % (2 * Math.PI); // normalize to 0..360
            α2 = (α2 + 2 * Math.PI) % (2 * Math.PI); // normalize to 0..360

            s = Math.Round(s, 3); // round to 1mm precision, round 3

            return (s, α1.ToDegrees(), α2.ToDegrees());
        }

        /// <summary>
        /// Calculate point directly using Vincenty's formulae
        /// </summary>
        /// <param name="origin">origin point</param>
        /// <param name="distance">distance</param>
        /// <param name="bearing">bearing</param>
        /// <param name="datum">datum</param>
        /// <returns>destination point, actual bearing</returns>
        private static (SanchaegGeoCoordinate point, double bearing) CalculateDirect(SanchaegGeoCoordinate origin, double distance, double bearing, Ellipsoid datum)
        {
            var φ1 = origin.Latitude.ToRadians();
            var λ1 = origin.Longitude.ToRadians();
            var α1 = bearing.ToRadians();
            var s = distance;

            var a = datum.MajorAxis;
            var b = datum.MinorAxis;
            var f = datum.Flattening;

            var sinα1 = Math.Sin(α1);
            var cosα1 = Math.Cos(α1);

            var tanU1 = (1 - f)*Math.Tan(φ1);
            var cosU1 = 1/Math.Sqrt((1 + tanU1*tanU1));
            var sinU1 = tanU1 * cosU1;
            var σ1 = Math.Atan2(tanU1, cosα1);
            var sinα = cosU1 * sinα1;
            var cosSqα = 1 - sinα * sinα;
            var uSq = cosSqα * (a * a - b * b) / (b * b);
            var A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
            var B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));

            double cos2σM, sinσ, cosσ, Δσ;

            var σ = s/(b*A);
            double σʹ;
            var iterations = 0;
            do
            {
                cos2σM = Math.Cos(2 * σ1 + σ);
                sinσ = Math.Sin(σ);
                cosσ = Math.Cos(σ);
                Δσ = B * sinσ * (cos2σM + B / 4 * (cosσ * (-1 + 2 * cos2σM * cos2σM) -
                    B / 6 * cos2σM * (-3 + 4 * sinσ * sinσ) * (-3 + 4 * cos2σM * cos2σM)));
                σʹ = σ;
                σ = s / (b * A) + Δσ;
            } while (Math.Abs(σ - σʹ) > 1e-12 && ++iterations < 200);
            if (iterations >= 200) throw new Exception("Formula failed to converge");

            var x = sinU1 * sinσ - cosU1 * cosσ * cosα1;
            var φ2 = Math.Atan2(sinU1 * cosσ + cosU1 * sinσ * cosα1, (1 - f) * Math.Sqrt(sinα * sinα + x * x));
            var λ = Math.Atan2(sinσ * sinα1, cosU1 * cosσ - sinU1 * sinσ * cosα1);
            var C = f / 16.0 * cosSqα * (4 + f * (4 - 3 * cosSqα));
            var L = λ - (1 - C) * f * sinα *
                (σ + C * sinσ * (cos2σM + C * cosσ * (-1 + 2 * cos2σM * cos2σM)));
            var λ2 = (λ1 + L + 3 * Math.PI) % (2 * Math.PI) - Math.PI;  // normalize to -180..+180

            var α2 = Math.Atan2(sinα, -x);
            α2 = (α2 + 2 * Math.PI) % (2 * Math.PI); // normalize to 0..360

            return (new SanchaegGeoCoordinate
            {
                Latitude = φ2.ToDegrees(),
                Longitude = λ2.ToDegrees()
            }, α2.ToDegrees());
        }
    }
}
