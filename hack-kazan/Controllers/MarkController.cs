using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using Hack.Domain.Entities;
using Hack.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace hack_kazan.Controllers;

public class MarkController : BaseController
{
    private readonly IMarkService _markService;

    public MarkController(IMarkService markService)
    {
        _markService = markService;
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
    [Route("get")]
    public async Task<ActionResult<Mark>> GetMarksAsync()
    {
        var mark = await _markService.GetAllAsync();
        return Ok(mark);
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