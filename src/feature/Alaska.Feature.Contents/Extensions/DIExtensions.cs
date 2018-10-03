using Alaska.Foundation.Core.Caching.Extensions;
using Alaska.Feature.Contents.Cache;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Alaska.Feature.Contents.Concrete;
using Alaska.Feature.Contents.Abstractions;
using Alaska.Foundation.Core.Messaging.Email.Interfaces;

namespace Alaska.Feature.Contents.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection AddJsonContentService(this IServiceCollection services, Action<JsonContentServiceOptions> setup = null)
        {
            var options = new JsonContentServiceOptions();
            setup?.Invoke(options);

            return services
                .AddSingleton(options)
                .AddMemoryCacheInstance<IContentCache, CmsContentsCache>()
                .AddScoped<IContentManager, JsonContentManager>()
                .AddScoped<IContentService, ContentService>();
        }
    }
}
