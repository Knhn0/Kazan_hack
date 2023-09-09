using Hack.Domain.Contracts;
using Hack.Domain.Dto;
using Hack.Domain.Entities;
using Hack.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace hack_kazan.Controllers;

public class AuthController : BaseController
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService, UserManager<User> userManager)
    {
        _userManager = userManager;
        _authService = authService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login(AuthorizationRequest req)
    {
        TryValidateModel(req);
        if (!ModelState.IsValid) return BadRequest("Invalid request.");

        try
        {
            TokenResponse response;
            if (req.Username.IsNullOrEmpty()) response = await _authService.LoginByEmail(req.Email, req.Password);
            else response = await _authService.LoginByUsername(req.Username, req.Password);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Exception = ex.Message });
        }
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegistrationRequest req)
    {
        TryValidateModel(req);
        if (!ModelState.IsValid) return BadRequest("Invalid request.");

        try
        {
            var result = await _authService.Register(req.Username, req.Password, req.Email);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Exception = ex.Message });
        }
    }
}

