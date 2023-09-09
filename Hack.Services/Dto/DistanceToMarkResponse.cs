using System.ComponentModel.DataAnnotations;
using Hack.Domain.Entities;

namespace Hack.Domain.Dto;

public class DistanceToMarkResponse
{
    public Mark Mark { get; set; }
    public double DistanceInMeters { get; set; }
}