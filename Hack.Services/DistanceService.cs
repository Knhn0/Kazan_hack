using Hack.Domain.Entities;
using Hack.Services.Interfaces;

namespace Hack.Services;

public class DistanceService : IDistanceService
{
    private readonly IMarkService _markService;

    public DistanceService(IMarkService markService)
    {
        _markService = markService;
    }
    
    public async Task<Tuple<Mark, double>> GetDistanceToMark(double latitude, double longitude, Mark mark)
    {
        double distance = GetDistanceBetweenCoordinates(latitude, longitude,
                mark.Latitude, mark.Longitude);
        return await Task.FromResult(Tuple.Create(mark, distance));
    }
    
    public async Task<Tuple<Mark, double>> GetDistanceToMark(double latitude, double longitude, int markId)
    {
        var candidate = await _markService.FindFirstAsync(mark => mark.Id == markId);
        if (candidate is null) throw new Exception("Mark is null");
        return await GetDistanceToMark(latitude, longitude, candidate);
    }

    public async Task<List<Mark>> GetMarksNearby(double latitude, double longitude, double radius)
    {

        var all = await _markService.GetAllAsync();
        var found = all.FindAll(mark =>
            GetDistanceBetweenCoordinates(latitude, longitude,
                mark.Latitude, mark.Longitude) <= radius);
        
        if (found.Count == 0) throw new Exception("No marks has been found nearby");
        return found;
    }
    
    private double GetDistanceBetweenCoordinates(double latitude1, double longitude1, double latitude2,
        double longitude2)
    {
        var coord1 = new Coordinates(latitude1, longitude1);
        var coord2 = new Coordinates(latitude2, longitude2);

        return coord1.DistanceTo(coord2);
    }
}

public class Coordinates
{
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public Coordinates(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}
public static class CoordinatesDistanceExtensions
{
    public static double DistanceTo(this Coordinates baseCoordinates, Coordinates targetCoordinates)
    {
        return DistanceTo(baseCoordinates, targetCoordinates, UnitOfLength.Kilometers) * 1000;
    }

    public static double DistanceTo(this Coordinates baseCoordinates, Coordinates targetCoordinates, UnitOfLength unitOfLength)
    {
        var baseRad = Math.PI * baseCoordinates.Latitude / 180;
        var targetRad = Math.PI * targetCoordinates.Latitude/ 180;
        var theta = baseCoordinates.Longitude - targetCoordinates.Longitude;
        var thetaRad = Math.PI * theta / 180;

        double dist =
            Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
            Math.Cos(targetRad) * Math.Cos(thetaRad);
        dist = Math.Acos(dist);

        dist = dist * 180 / Math.PI;
        dist = dist * 60 * 1.1515;

        return unitOfLength.ConvertFromMiles(dist);
    }
}

public class UnitOfLength
{
    public static UnitOfLength Kilometers = new UnitOfLength(1.609344);
    public static UnitOfLength NauticalMiles = new UnitOfLength(0.8684);
    public static UnitOfLength Miles = new UnitOfLength(1);

    private readonly double _fromMilesFactor;

    private UnitOfLength(double fromMilesFactor)
    {
        _fromMilesFactor = fromMilesFactor;
    }

    public double ConvertFromMiles(double input)
    {
        return input*_fromMilesFactor;
    }
} 