using Hack.Domain.Dto;
using Hack.Services.Contracts;
using Hack.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hack_kazan.Controllers;

[Authorize]
public class DistanceController : BaseController
{
    private readonly IDistanceService _distanceService;

    public DistanceController(IDistanceService distanceService)
    {
        _distanceService = distanceService;
    }

    [HttpPost("get-to-mark")]
    public async Task<ActionResult> GetDistanceToMark(GetDistanceRequest req)
    {
        TryValidateModel(req);
        if (!ModelState.IsValid || !req.MarkId.HasValue) return BadRequest("Invalid request.");
        
        try
        {
            var tuple = await _distanceService.GetDistanceToMark(req.Latitude, req.Longitude, req.MarkId.GetValueOrDefault());
            var result = new DistanceToMarkResponse
            {
                Mark = tuple.Item1,
                DistanceInMeters = tuple.Item2
            };
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("get-nearby")]
    public async Task<ActionResult> GetNearbyMarks(GetDistanceRequest req)
    {
        TryValidateModel(req);
        if (!ModelState.IsValid || !req.Radius.HasValue) return BadRequest("Invalid request.");
        
        try
        {
            var nearby = await _distanceService.GetMarksNearby(req.Latitude, req.Longitude, req.Radius.GetValueOrDefault());

            return Ok(new MarksNearbyResponse
            {
                Marks = nearby
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}