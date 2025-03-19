namespace Sanchaeg.Tests
{
    /// <summary>
    /// Test helpers
    /// </summary>
    public static class TestHelpers
    {
        /// <summary>
        /// Check if two GeoCoordinates are approximately equal
        /// </summary>
        /// <param name="a">first GeoCoordinate</param>
        /// <param name="b">second GeoCoordinate</param>
        /// <returns>true when the two GeoCoordinates are equal within the defined CoordinateEpsilons</returns>
        public static bool ApproximatelyEquals(SanchaegGeoCoordinate a, SanchaegGeoCoordinate b) =>
            Math.Abs(a.Latitude - b.Latitude) < Precision.CoordinateEpsilon &&
            Math.Abs(a.Longitude - b.Longitude) < Precision.CoordinateEpsilon;
    }
}
