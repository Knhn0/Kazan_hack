using Hack.Domain.Dto;
using Hack.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponse> LoginByUsername(string username, string password);
        Task<TokenResponse> LoginByEmail(string email, string password);
        Task<TokenResponse> GetRefreshToken(TokenResponse token);
        Task<TokenResponse> Register(string username, string password, string email);
    }
}
