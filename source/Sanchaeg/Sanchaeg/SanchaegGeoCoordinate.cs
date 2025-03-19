namespace Sanchaeg
{
    /// <summary>
    /// Represents a geographic coordinate
    /// </summary>
    /// <param name="Latitude">Coordinate's latitude component</param>
    /// <param name="Longitude">Coordinate's longitude component</param>
    public record SanchaegGeoCoordinate(double Latitude = 0.0, double Longitude = 0.0);
}
