using Should;

namespace Sanchaeg.Tests
{
    [TestClass]
    public sealed class GeographyTests
    {
        /*
         * useful links
         *
         * testing multiple coordinate results
         * hamstermap - http://www.hamstermap.com/quickmap.php
         *
         */

        [TestMethod]
        public void InverseTest1()
        {
            // example from vincenty page http://www.movable-type.co.uk/scripts/latlong-vincenty.html#datums

            // founders peak
            var point1 = new SanchaegGeoCoordinate
            {
                Latitude = -37.951033,
                Longitude = 144.424868
            };

            // buninyong
            var point2 = new SanchaegGeoCoordinate
            {
                Latitude = -37.652821,
                Longitude = 143.926495
            };

            /*
             * d	54 972.271 m
             * α1	306°52′05.37″
             * α2	127°10′25.07″ (≡ 307°10′25.07″ p1→p2)
             */

            point1.CalculateDistanceBetween(point2).ShouldEqual(54972.241, Precision.DistanceEpsilon);
        }

        [TestMethod]
        public void InverseTest2()
        {
            // mountain
            var point1 = new SanchaegGeoCoordinate
            {
                Latitude = 47.483580,
                Longitude = 19.015257
            };

            // BUD liszt ferenc airport
            var point2 = new SanchaegGeoCoordinate
            {
                Latitude = 47.438437,
                Longitude = 19.252274
            };

            /*
             * d	18562 m
             */

            point1.CalculateDistanceBetween(point2).ShouldEqual(18562.0, Precision.DistanceEpsilon);
        }

        [TestMethod]
        public void DirectTest1()
        {
            // mountain
            var point1 = new SanchaegGeoCoordinate
            {
                Latitude = 47.483580,
                Longitude = 19.015257
            };

            var dest = point1.CalculateDestinationPoint(1000.0, 0.0);

            dest.CalculateDistanceBetween(point1).ShouldEqual(1000.0, Precision.DistanceEpsilon);
            dest.CalculateActualBearing(1000.0, 0.0).ShouldEqual(0.0, Precision.DistanceEpsilon);
        }

        [TestMethod]
        public void DirectTest2()
        {
            // mountain
            var point1 = new SanchaegGeoCoordinate
            {
                Latitude = 47.483580,
                Longitude = 19.015257
            };

            var dest1 = point1.CalculateDestinationPoint(1000.0, 0.0);
            var dest2 = point1.CalculateDestinationPoint(1000.0, 180.0);

            dest1.CalculateDistanceBetween(dest2).ShouldEqual(2000.0, Precision.DistanceEpsilon);
        }

        [TestMethod]
        public void DirectTest3()
        {
            // mountain
            var point1 = new SanchaegGeoCoordinate
            {
                Latitude = 47.483580,
                Longitude = 19.015257
            };

            var dest1 = point1.CalculateDestinationPoint(100000.0, -90.0);
            var dest2 = point1.CalculateDestinationPoint(100000.0, 90.0);

            dest1.CalculateDistanceBetween(dest2).ShouldEqual(200000.0, Precision.DistanceEpsilon);
        }

        [TestMethod]
        public void DirectTest4()
        {
            // mountain
            var point1 = new SanchaegGeoCoordinate
            {
                Latitude = 47.483580,
                Longitude = 19.015257
            };

            var dest1 = point1.CalculateDestinationPoint(0.0, 0.0);
            var dest2 = point1.CalculateDestinationPoint(0.0, 90.0);
            var dest3 = point1.CalculateDestinationPoint(0.0, 180.0);
            var dest4 = point1.CalculateDestinationPoint(0.0, 270.0);

            TestHelpers.ApproximatelyEquals(point1, dest1).ShouldBeTrue();
            TestHelpers.ApproximatelyEquals(point1, dest2).ShouldBeTrue();
            TestHelpers.ApproximatelyEquals(point1, dest3).ShouldBeTrue();
            TestHelpers.ApproximatelyEquals(point1, dest4).ShouldBeTrue();
        }
    }
}
