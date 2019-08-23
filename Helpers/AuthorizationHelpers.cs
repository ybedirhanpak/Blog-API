using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog_Project.Models;

namespace Blog_Project.Helpers
{
    public class AuthorizationHelpers
    {
        /**
         * Checks if given token contains information about admin
         */
        public static bool IsAdmin(ClaimsPrincipal tokenUser)
        {
            return tokenUser.HasClaim(u => u.Type == ClaimTypes.Role) &&
                   tokenUser.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Role)?.Value == Role.Admin;
        }

        /**
         * Checks if given token contains information about the user which has given id.
         */
        public static bool IsAuthorizedUser(ClaimsPrincipal tokenUser, string id)
        {
            return tokenUser.HasClaim(u => u.Type == "Id") &&
                   tokenUser.Claims.FirstOrDefault(u => u.Type == "Id")?.Value == id;
        }
    }
}
