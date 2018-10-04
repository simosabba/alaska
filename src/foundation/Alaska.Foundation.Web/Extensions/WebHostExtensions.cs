using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Data.SqlClient;

namespace Microsoft.AspNetCore.Hosting
{
    public static class WebHostExtensions
    {
        public static IWebHost EnsureDatabase(this IWebHost webHost, string connectionStringName)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<IWebHost>>();

                var configuration = services.GetRequiredService<IConfiguration>();

                try
                {
                    var connectionString = configuration.GetConnectionString(connectionStringName);
                    if (string.IsNullOrWhiteSpace(connectionString))
                        throw new InvalidOperationException($"Connection string {connectionStringName} not found");

                    var connectionBuilder = new SqlConnectionStringBuilder(connectionString);
                    var databaseName = connectionBuilder.InitialCatalog;
                    connectionBuilder.InitialCatalog = "master";
                    var masterConnectionString = connectionBuilder.ConnectionString;

                    using (var connection = new SqlConnection(masterConnectionString))
                    using (var command = new SqlCommand($"if not exists(select * from sys.databases where name = '{databaseName}') create database [{databaseName}]", connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while initializing database for connection string {connectionStringName}");
                }
            }

            return webHost;
        }

        public static IWebHost MigrateDbContext<TContext>(this IWebHost webHost, Action<TContext,IServiceProvider> seeder) where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<TContext>>();

                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");

                    var retry = Policy.Handle<Exception>()
                         .WaitAndRetry(new TimeSpan[]
                         {
                             TimeSpan.FromSeconds(3),
                             TimeSpan.FromSeconds(5),
                             TimeSpan.FromSeconds(8),
                         });

                    retry.Execute(() =>
                    {
                        //if the sql server container is not created on run docker compose this
                        //migration can't fail for network related exception. The retry options for DbContext only 
                        //apply to transient exceptions.

                        context.Database
                        .Migrate();

                        seeder(context, services);
                    });
                  
                    logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while migrating the database used on context {typeof(TContext).Name}");
                }
            }

            return webHost;
        }
    }
}
