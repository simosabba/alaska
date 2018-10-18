using Alaska.Foundation.Godzilla.Services;
using Alaska.Foundation.Godzilla.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GodzillaDIExtension
    {
        public static IServiceCollection AddEntityContext<T>(this IServiceCollection services, Action<EntityContextOptions<T>> setupAction = null)
            where T : EntityContext
        {
            var options = new EntityContextOptions<T>();
            setupAction?.Invoke(options);

            return services
                .AddSingleton(options)
                .AddScoped<T>();
        }
    }
}
