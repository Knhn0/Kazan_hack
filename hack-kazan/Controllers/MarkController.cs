using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using Hack.Domain.Entities;
using Hack.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace hack_kazan.Controllers;

[Authorize]
public class MarkController : BaseController
{
    private readonly IMarkService _markService;
    private readonly IUserService _userService;

    public MarkController(IMarkService markService, IUserService userService)
    {
        _markService = markService;
        _userService = userService;
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
    [Route("get/{completed}")]
    public async Task<ActionResult<Mark>> GetMarksAsync(bool completed)
    {
        var usernameClaim = User
            .Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

        if (usernameClaim is null) return BadRequest("bruh");
        var user = await _userService.GetUserManager().FindByNameAsync(usernameClaim.Value);
        var marks = await _markService.FindManyAsync(mark => user.MarksDiscovered.Contains(mark.Id));
        return Ok(marks);
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

    [HttpPost]
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
    }
        
}

public class aSFlksf
{
    public int id;
    public double latitude, longitude;
}