using System.ComponentModel.DataAnnotations;
using Hack.Domain.Entities;
using Hack.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace hack_kazan.Controllers;

public class MarkChainController : BaseController
{
    private readonly IMarkChainService _markChainService;

    public MarkChainController(IMarkChainService markChainService)
    {
        _markChainService = markChainService;
    }
    
    [HttpGet("get")]
    public async Task<ActionResult<MarkChain>> GetMarkChains()
    {
        var markChain = await _markChainService.GetAllAsync();
        return Ok(markChain);
    }
    
    [HttpGet("get/{id}")]
    public async Task<ActionResult<MarkChain>> GetMarkChain([Required] int id)
    {
        try
        {
            var markChain = await _markChainService.GetByIdAsync(id);
            return Ok(markChain);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("update")]
    public async Task<ActionResult<MarkChain>> UpdateUserAsync([Required] int id)
    {
        try
        {
            var candidate = await _markChainService.GetByIdAsync(id);
            var markChain = await _markChainService.UpdateAsync(candidate);
            return Ok(markChain);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("create")]
    public async Task<ActionResult<MarkChain>> CreateMarkChainAsync(MarkChain markChain)
    {
        try
        {
            var result = await _markChainService.CreateAsync(markChain);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Exception = ex.Message });
        }
    }

    [HttpPost("append")]
    public async Task<ActionResult<MarkChain>> AppendMarkToChain([Required] int chainId, [Required] Mark mark)
    {
        if (!ModelState.IsValid) return BadRequest("Invalid request.");
        try
        {
            await _markChainService.AppendMark(chainId, mark);
            return Ok(await _markChainService.GetByIdAsync(chainId));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("remove")]
    public async Task<ActionResult<MarkChain>> RemoveMarkFromChain([Required] int chainId, [Required] int markId)
    {
        if (!ModelState.IsValid) return BadRequest("Invalid request.");
        try
        {
            await _markChainService.RemoveMark(chainId, markId);
            return Ok(await _markChainService.GetByIdAsync(chainId));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpDelete]
    public async Task<ActionResult<MarkChain>> RemoveMarkFromChain([Required] MarkChain markChain)
    {
        if (!ModelState.IsValid) return BadRequest("Invalid request.");
        try
        {
            await _markChainService.RemoveAsync(markChain);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}