using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using Hack.Domain.Entities;
using Hack.Services.Contracts;
using Hack.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace hack_kazan.Controllers;

[Authorize]
public class MarkController : BaseController
{
    private readonly IMarkService _markService;
    private readonly IUserService _userService;
    private readonly IDistanceService _distanceService;

    public MarkController(IMarkService markService, IUserService userService, IDistanceService distanceService)
    {
        _markService = markService;
        _userService = userService;
        _distanceService = distanceService;
    }
    
    [HttpGet]
    [Route("get")]
    public async Task<ActionResult<Mark>> GetMarksAsync()
    {
        var marks = await _markService.GetAllAsync();
        return Ok(marks);
    }

    [HttpGet]
    [Route("get/{id}")]
    public async Task<ActionResult<Mark>> GetMarkAsync(int id)
    {
        if (id.ToString().IsNullOrEmpty())
        {
            return BadRequest("Not valid id");
        }

        var mark = await _markService.GetByIdAsync(id);
        return Ok(mark);
    }
    
    [HttpGet]
    [Route("get-completed")]
    public async Task<ActionResult<Mark>> GetCompletedMarks()
    {
        var usernameClaim = User
            .Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

        if (usernameClaim is null) return BadRequest("bruh");
        
        var user = await _userService.GetUserManager().FindByNameAsync(usernameClaim.Value);
        if (user == null) return BadRequest("user == null");
        var completed = await _userService.GetMarksDiscovered(Guid.Parse(user.Id));
        
        return Ok(completed);
    }

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<Mark>> CreateMarkAsync(Mark mark)
    {
        var resp = await _markService.CreateAsync(mark);
        return Ok(resp);
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<ActionResult<Mark>> DeleteMarkAsync (int id)
    {
        if (id.ToString().IsNullOrEmpty())
        {
            return BadRequest("Not valid id");
        }

        var mark = await _markService.GetByIdAsync(id);
        if (mark != null) await _markService.RemoveAsync(mark);
        return Ok("Removed");
    }

    /*[HttpPost]
    [Route("change-emoji-title/{id}")]
    public async Task<ActionResult<Mark>> EditMarkAsync(int id, [FromBody] string emojified)
    {
        var mark = (await GetMarkAsync(id)).Value!;
        mark.EmojifiedTitle = emojified;
        await _markService.SaveChangesAsync();
        return mark;
    }

    [HttpPost]
    [Route("change-coords")]
    public async Task<ActionResult<Mark>> EditMarkAsync(aSFlksf req)
    {
        var mark = (await GetMarkAsync(req.id)).Value!;
        mark.Latitude = req.latitude;
        mark.Longitude = req.longitude;
        await _markService.SaveChangesAsync();
        return mark;
    }*/

    [HttpPost]
    [Route("reach")]
    public async Task<ActionResult<List<Mark>>> Reach(GetDistanceRequest req)
    {
        var usernameClaim = User
            .Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

        if (usernameClaim is null) return BadRequest("bruh");
        
        var user = await _userService.GetUserManager().FindByNameAsync(usernameClaim.Value);
        
        TryValidateModel(req);
        if (!ModelState.IsValid) return BadRequest("Bad request");

        double radius = 200;
            
        var nearby = await _distanceService.GetMarksNearby(req.Latitude, req.Longitude, radius);

        foreach (var mark in nearby)
        {
            if (!(await _userService.IsMarkDiscovered(Guid.Parse(user.Id), mark.Id)))
                await _userService.AddMarkDiscovered(user.UserName, mark.Id);
        }
        

        await _userService.GetUserManager().UpdateAsync(user);

        return nearby;
    }
}