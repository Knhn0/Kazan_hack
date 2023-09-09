using Hack.Domain.Dto;
using Hack.Domain.Entities;
using Hack.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace hack_kazan.Controllers
{
    public class TokenController : BaseController // это должно в идеале запускаться на отдельном сервере
    {
        /*
         * Это запускается в отдельном серверном приложении в идеале, поэтому
         * нужно менять токен сразу в двух местах: в конфиге и здесь
        */

        private readonly IAuthService _authService;
        private readonly ILogger<TokenController> _logger;
        private readonly UserManager<User> _userManager;

        public TokenController(IAuthService authService, ILogger<TokenController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<ActionResult> RefreshToken(TokenResponse tokenResponse)
        {
            try
            {
                var result = await _authService.GetRefreshToken(tokenResponse);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { Exception = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<ActionResult> Revoke(string username)
        {
            var candidate = await _userManager.FindByNameAsync(username);
            if (candidate is null) return BadRequest("Invalid username");

            candidate.RefreshToken = null;
            await _userManager.UpdateAsync(candidate);

            return Ok("Refresh token revoked");
        }
    }
}
