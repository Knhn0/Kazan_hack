using System.ComponentModel.DataAnnotations;
using Hack.Domain.Entities;
using Hack.Services;
using Hack.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace hack_kazan.Controllers;

public class UserController : BaseController
{
    private readonly UserManager<User> _userManager;
    private readonly IUserService _userService;
    private readonly IMarkService _markService;

    public UserController(UserManager<User> userManager, IUserService userService, IMarkService markService)
    {
        _userManager = userManager;
        _userService = userService;
        _markService = markService;
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
    public async Task<ActionResult<User>> UpdateUserAsync([Required] Guid userId, User user)
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

    [HttpGet]
    [Route("get-by-username/{username}")]
    public async Task<ActionResult<User>> GetUserByUsername(string username)
    {
        var res = username.IsNullOrEmpty();
        if (res)
        {
            return BadRequest("User not found");
        }

        var resp = await _userService.GetUserByUsernameAsync(username);
        return Ok(resp);
    }

    [HttpPost]
    [Route("add-mark")]
    public async Task<ActionResult<User>> AddMark(string username, int markId)
    {
        var user = await _userManager.FindByNameAsync(username);
        var mark = await _markService.GetByIdAsync(markId);
        if (mark != null)
        {
            if (user.MarksDiscovered == null) user.MarksDiscovered = new List<int>();
            user.MarksDiscovered.Add(markId);
        }
        await _userManager.UpdateAsync(user);
        return Ok(user.MarksDiscovered);
    }
}