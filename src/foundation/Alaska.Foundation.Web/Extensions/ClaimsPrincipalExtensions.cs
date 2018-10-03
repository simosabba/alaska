using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Alaska.Foundation.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetSubjectId(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                throw new InvalidOperationException("Cannot create profle instance from anonymous user");

            return user.Claims.FirstOrDefault(x => x.Type == "sub")?.Value ??
                throw new InvalidOperationException("Missing subject claim");
        }
    }
}
