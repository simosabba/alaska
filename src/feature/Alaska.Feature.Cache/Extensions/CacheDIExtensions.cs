using Alaska.Feature.Cache.Controllers;
using Alaska.Foundation.Core.Caching.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Alaska.Feature.Cache.Extensions
{
    public static class CacheDIExtensions
    {
        public static IServiceCollection AddAlaskaCache(this IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddApplicationPart(typeof(CacheController).Assembly);

            return services
                .AddCacheService();
        }
    }
}
