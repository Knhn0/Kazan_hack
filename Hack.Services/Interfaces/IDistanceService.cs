using Hack.Domain.Entites;

namespace Hack.Services.Interfaces;

public interface IDistanceService
{
    Task<Tuple<Mark, double>> GetDistanceToMark(double latitude, double longitude, Mark mark);
    Task<Tuple<Mark, double>> GetDistanceToMark(double latitude, double longitude, int markId);
    Task<List<Mark>> GetMarksNearby(double latitude, double longitude, double radius);
}