using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddAllowAnyOriginsCorsPolicy(this IServiceCollection services)
        {
            var s = services.AddCors(options =>
                options.AddPolicy("AllowAnyOrigin",
                    builder =>
                    {
                        builder
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                    }));
            return s;
        }
    }
}
