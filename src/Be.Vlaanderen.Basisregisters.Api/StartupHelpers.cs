namespace Be.Vlaanderen.Basisregisters.Api
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AggregateSource.SqlStreamStore;
    using Autofac;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Polly;
    using Serilog;
    using SqlStreamStore;

    public static class StartupHelpers
    {
        public const string AllowAnyOrigin = "AllowAnyOrigin";
        public const string AllowSpecificOrigin = "AllowSpecificOrigin";

        public static void RegisterApplicationLifetimeHandling(
            IContainer applicationContainer,
            IHostApplicationLifetime appLifetime)
        {
            appLifetime.ApplicationStarted.Register(() => Log.Information("Application started."));

            appLifetime.ApplicationStopping.Register(() =>
            {
                Log.Information("Application stopping.");
                Log.CloseAndFlush();
            });

            appLifetime.ApplicationStopped.Register(applicationContainer.Dispose);

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                appLifetime.StopApplication();

                // Don't terminate the process immediately, wait for the Main thread to exit gracefully.
                eventArgs.Cancel = true;
            };
        }

        public static void EnsureSqlStreamStoreSchema<T>(
            MsSqlStreamStore streamStore,
            ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<T>();

            Policy
                .Handle<SqlException>()
                .WaitAndRetry(
                    5,
                    retryAttempt =>
                    {
                        var value = Math.Pow(2, retryAttempt) / 4;
                        var randomValue = new Random().Next((int)value * 3, (int)value * 5);
                        logger.LogInformation("Retrying after {Seconds} seconds...", randomValue);
                        return TimeSpan.FromSeconds(randomValue);
                    })
                .Execute(() =>
                {
                    logger.LogInformation("Ensuring the sql stream store schema.");

                    var checkSchemaResult = streamStore.CheckSchema().GetAwaiter().GetResult();
                    if (!checkSchemaResult.IsMatch())
                        streamStore.CreateSchema().GetAwaiter().GetResult();
                });
        }

        public static void EnsureSqlSnapshotStoreSchema<T>(
            MsSqlSnapshotStore streamStore,
            ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<T>();

            Policy
                .Handle<SqlException>()
                .WaitAndRetry(
                    5,
                    retryAttempt =>
                    {
                        var value = Math.Pow(2, retryAttempt) / 4;
                        var randomValue = new Random().Next((int)value * 3, (int)value * 5);
                        logger.LogInformation("Retrying after {Seconds} seconds...", randomValue);
                        return TimeSpan.FromSeconds(randomValue);
                    })
                .Execute(() =>
                {
                    logger.LogInformation("Ensuring the sql snapshot store schema.");

                    streamStore.CreateTable().GetAwaiter().GetResult();
                });
        }

        public static async Task CheckDatabases(
            HealthCheckService healthCheckService,
            string databaseTag,
            ILoggerFactory loggerFactory,
            int retryCount = 5,
            int delaySeconds = 2)
        {
            var logger = loggerFactory.CreateLogger("CheckDatabasesLogger");

            string FormatHealthReport(HealthReport healthReport)
            {
                var entries = healthReport
                    .Entries
                    .Where(x => x.Value.Status != HealthStatus.Healthy)
                    .Select(x => $"{x.Key} - {x.Value.Exception?.Message}");

                return $"\n\t* {string.Join("\n\t* ", entries)}";
            }

            HealthReport? result = null;

            await Policy.Handle<Exception>()
                .WaitAndRetryAsync(retryCount, _ => TimeSpan.FromSeconds(delaySeconds),
                (_, timespan) =>
                {
                    logger.LogInformation($"Retrying database healthcheck after {timespan.Seconds} seconds.");
                })
                .ExecuteAsync(async () =>
                {
                    result = await healthCheckService.CheckHealthAsync(x => x.Tags.Contains(databaseTag));

                    if (result.Status != HealthStatus.Healthy)
                        throw new InvalidOperationException($"Databases healthcheck failed.");
                });

            if (result!.Status != HealthStatus.Healthy)
                throw new InvalidOperationException($"Databases not ready after {retryCount} retries, healthreport: {FormatHealthReport(result)}");
        }
    }
}
