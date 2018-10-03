using Alaska.Foundation.Core.EventBus;
using Alaska.Foundation.Core.EventBus.Abstractions;
using Alaska.Foundation.Extensions.EventBus.RabbitMQBus;
using Alaska.Foundation.Extensions.EventBus.RabbitMQBus.Abstractions;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMqDependencyInjectionExtensions
    {
        public static IServiceCollection AddRabbitMQEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddTransient<IIntegrationEventsService, DefaultIntegrationEventsService>()
                .AddRabbitMQConnection(configuration)
                .AddRabbitMQInstance(configuration);
        }

        public static IServiceCollection AddRabbitMQConnection(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = configuration["EventBus:Connection"],
                    UserName = configuration["EventBus:UserName"],
                    Password = configuration["EventBus:Password"],
                    VirtualHost = "/",
                };
                
                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBus:RetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBus:RetryCount"]);
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

        }

        private static IServiceCollection AddRabbitMQInstance(this IServiceCollection services, IConfiguration configuration)
        { 
            var subscriptionClientName = configuration["EventBus:SubscriptionClientName"];

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBus:RetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBus:RetryCount"]);
                }

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }
    }
}
