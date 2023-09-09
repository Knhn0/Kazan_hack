using Hack.DAL.Interfaces;
using Hack.Domain;
using Hack.Domain.Contracts;
using Hack.Domain.Dto;
using Hack.Domain.Entities;
using Hack.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hack.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        { 
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
       
        public async Task<TokenResponse> GetRefreshToken(TokenResponse token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
            string username = principal.Identity!.Name!;
            var candidate = await _userManager.FindByNameAsync(username);

            if (candidate is null || candidate.RefreshToken != token.RefreshToken) 
            {
                throw new Exception("Invalid access token");
            }

            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, candidate.UserName!),
               new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var newAccessToken = GenerateToken(authClaims);
            var newRefreshToken = GenerateRefreshToken();

            candidate.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(candidate);

            return new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<TokenResponse> LoginByEmail(string email, string password)
        {
            var candidate = await _userManager.FindByEmailAsync(email);

            if (candidate is null)
            {
                throw new Exception("Invalid email");
            }

            if (!await _userManager.CheckPasswordAsync(candidate, password))
            {
                throw new Exception("Invalid password");
            }

            var userRoles = await _userManager.GetRolesAsync(candidate);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, candidate.UserName!),
               new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var tokenDto = new TokenResponse
            {
                AccessToken = GenerateToken(authClaims),
                RefreshToken = GenerateRefreshToken(),
            };

            var refreshTokenValidityInDays = Convert.ToInt64(_configuration["JwtSettings:RefreshTokenValidityInDays"]);
            candidate.RefreshToken = tokenDto.RefreshToken;
            candidate.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
            await _userManager.UpdateAsync(candidate);

            return tokenDto;
        }

        public async Task<TokenResponse> LoginByUsername(string username, string password)
        {
            var candidate = await _userManager.FindByNameAsync(username);

            if (candidate is null)
            {
                throw new Exception("Invalid username");
            }

            if (!await _userManager.CheckPasswordAsync(candidate, password))
            {
                throw new Exception("Invalid password");
            }

            var userRoles = await _userManager.GetRolesAsync(candidate);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, candidate.UserName!),
               new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var tokenDto = new TokenResponse
            {
                AccessToken = GenerateToken(authClaims),
                RefreshToken = GenerateRefreshToken(),
            };

            var refreshTokenValidityInDays = Convert.ToInt64(_configuration["JwtSettings:RefreshTokenValidityInDays"]);
            candidate.RefreshToken = tokenDto.RefreshToken;
            candidate.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
            await _userManager.UpdateAsync(candidate);

            return tokenDto;
        }

        public async Task<TokenResponse> Register(string username, string password, string email)
        {
            var candidate = await _userManager.FindByNameAsync(username);

            if (candidate is not null)
            {
                throw new Exception("User already exists");
            }

            if (username.IsNullOrEmpty() || password.IsNullOrEmpty() || email.IsNullOrEmpty())
            {
                throw new Exception("Invalid request");
            }

            var user = new User
            {
                Email = email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = username
            };

            var createResult = await _userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(err => err.Description);
                throw new Exception("User creation failed: " + String.Join(", ", errors));
            }

            string role = Roles.Default;
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            if (await _roleManager.RoleExistsAsync(role))
                await _userManager.AddToRoleAsync(user, role);

            return await LoginByUsername(username, password);
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
            var tokenExpiryTimeInHour = Convert.ToInt64(_configuration["JwtSettings:TokenExpiryTimeInHour"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JwtSettings:ValidIssuer"],
                Audience = _configuration["JwtSettings:ValidAudience"],
                Expires = DateTime.UtcNow.AddHours(tokenExpiryTimeInHour),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
