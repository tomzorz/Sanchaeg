namespace Sanchaeg
{
    /// <summary>
    /// Precision values for various calculations
    /// </summary>
    public static class Precision
    {
        /// <summary>
        /// Precision of WGS-84, 1.0 meter
        /// </summary>
        public static double DistanceEpsilon => 1.0;

        /// <summary>
        /// Precision of WGS-84, 1.0 meter in degrees
        /// </summary>
        public static double CoordinateEpsilon => 0.000008889;

        /// <summary>
        /// Twice the angular size of a 10.0 meters wide object seen from 10.0 kilometers
        /// </summary>
        public static double AngularEpsilon => 0.114592;

        /// <summary>
        /// Radius the AreaEpsilon is based on
        /// </summary>
        public static double AreaEpsilonBase => 100.0;

        /// <summary>
        /// 1% of the area of circle with a 100.0 meters large radius
        /// </summary>
        public static double AreaEpsilon => 314.156;
    }
}
