using Alaska.Foundation.Godzilla.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class GodzillaAppBuilderExtensions
    {
        public static IApplicationBuilder UseGodzilla(this IApplicationBuilder applicationBuilder)
        {
            var builder = applicationBuilder.ApplicationServices.GetRequiredService<EntityContextBuilder>();
            builder.Build();

            return applicationBuilder;
        }
    }
}
