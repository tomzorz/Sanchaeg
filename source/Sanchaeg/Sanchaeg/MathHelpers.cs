namespace Sanchaeg
{
    /// <summary>
    /// Math helper methods
    /// </summary>
    internal static class MathHelpers
    {
        /// <summary>
        /// Convert degrees to radians
        /// </summary>
        /// <param name="d">degrees</param>
        /// <returns>radians</returns>
        public static double ToRadians(this double d) => d * Math.PI / 180.0;

        /// <summary>
        /// Convert radians to degrees
        /// </summary>
        /// <param name="r">radians</param>
        /// <returns>degrees</returns>
        public static double ToDegrees(this double r) => r * 180.0 / Math.PI;
    }
}
