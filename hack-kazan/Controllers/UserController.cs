using System.ComponentModel.DataAnnotations;
using Hack.Domain.Entities;
using Hack.Services;
using Hack.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hack_kazan.Controllers;

public class UserController : BaseController
{
    private readonly UserManager<User> _userManager;
    private readonly IUserService _userService;

    public UserController(UserManager<User> userManager, IUserService userService)
    {
        _userManager = userManager;
        _userService = userService;
    }

    [HttpGet("get/{userId}")]
    public async Task<ActionResult<User>> GetUserAsync([Required] string userId)
    {
        var res = Guid.TryParse(userId, out _);
        if (!res)
        {
            return BadRequest("Id is not valid");
        }

        var user = await _userManager.FindByIdAsync(userId);
        return Ok(user);
    }
    
    [HttpGet("get")]
    public async Task<ActionResult<User>> GetAllUsersAsync()
    {
        var user = await _userManager.Users.ToListAsync();
        return Ok(user);
    }

    [HttpPut("update")]
    public async Task<ActionResult<User>> UpdateUserAsync([Required]Guid userId, User user)
    {
        var res = Guid.TryParse(userId.ToString(), out _);
        if (!res)
        {
            return BadRequest("Id is not valid");
        }
        user = await _userService.FullyUserUpdate(userId, user);
        return Ok(user);
    }

    [HttpDelete("delete/{userId}")]
    public async Task<ActionResult> DeleteUserAsync([Required] string userId)
    {
        var res = Guid.TryParse(userId, out _);
        if (!res)
        {
            return BadRequest("Id is not valid");
        }

        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null)
        {
            return BadRequest("User not found");
        }
        var response = _userManager.DeleteAsync(user);
        return Ok("User successfully deleted");
    }
    
    
}