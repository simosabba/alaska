using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class CorsExtensions
    {
        public static IApplicationBuilder UseAllowAnyOriginCorsPolicy(this IApplicationBuilder app)
        {
            return app.UseCors("AllowAnyOrigin");
        }
    }
}
