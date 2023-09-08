using System.ComponentModel.DataAnnotations;

namespace Hack.Services.Contracts;

public class GetDistanceRequest
{
    [Required] public double Latitude { get; set; }
    [Required] public double Longitude { get; set; }
    public int? MarkId { get; set; }
    public double? Radius { get; set; }
}