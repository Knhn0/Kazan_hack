using GeoCoordinatePortable;

namespace Hack.Gameplay.Utils;

public static class CoordinateCalculator
{
    public static double GetDistanceBetweenCoordinates(GeoCoordinate cord1, GeoCoordinate cord2)
    {
        return cord1.GetDistanceTo(cord2);
    }

    public static double GetDistanceBetweenCoordinates(double latitude1, double longitude1, double latitude2,
        double longitude2)
    {
        return GetDistanceBetweenCoordinates(new GeoCoordinate(latitude1, longitude1),
            new GeoCoordinate(latitude2, longitude2));
    }
}