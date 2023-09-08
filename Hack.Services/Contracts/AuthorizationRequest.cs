using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack.Domain.Contracts
{
    public class AuthorizationRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        [EmailAddress] public string? Email { get; set; }
    }
}
