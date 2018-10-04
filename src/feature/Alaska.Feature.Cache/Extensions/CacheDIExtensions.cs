using Alaska.Feature.Cache.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Feature.Cache.Extensions
{
    public static class CacheDIExtensions
    {
        public static IMvcCoreBuilder AddAlaskaCache(this IServiceCollection services)
        {
            return services
                .AddMvcCore()
                .AddApplicationPart(typeof(CacheController).Assembly);
        }
    }
}
