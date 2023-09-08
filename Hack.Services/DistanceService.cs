using Hack.Domain.Entites;
using Hack.Gameplay.Utils;
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
        double distance = CoordinateCalculator.GetDistanceBetweenCoordinates(latitude, longitude,
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
        var candidates = await _markService.FindManyAsync(mark =>
            CoordinateCalculator.GetDistanceBetweenCoordinates(latitude, longitude,
                mark.Latitude, mark.Longitude) <= radius);
        if (candidates.Count == 0) throw new Exception("No marks has been found nearby");
        return candidates;
    }
}