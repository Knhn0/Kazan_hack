using System.ComponentModel.DataAnnotations;
using Hack.Domain.Entites;

namespace Hack.Domain.Dto;

public class DistanceToMarkResponse
{
    public Mark Mark { get; set; }
    public double DistanceInMeters { get; set; }
}