using Alaska.Foundation.Godzilla.Services;
using Alaska.Foundation.Godzilla.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GodzillaDIExtension
    {
        public static IServiceCollection AddGodzilla(this IServiceCollection services, Action<GodzillaOptions> setupAction = null)
        {
            var options = new GodzillaOptions();
            setupAction?.Invoke(options);

            return services
                .AddSingleton(options)
                .AddSingleton<EntityCollectionResolver>()
                .AddSingleton<EntityContextBuilder>()
                .AddScoped<EntityContext>();
        }
    }
}
