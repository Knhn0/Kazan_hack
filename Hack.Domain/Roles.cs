using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack.Domain
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Default = "Default";

        public static bool IsRoleValid(string role)
        {
            return typeof(Roles).GetFields().Any(f => f.Name == role);
        }
    }
}
