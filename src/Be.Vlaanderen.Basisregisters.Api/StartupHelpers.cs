namespace Be.Vlaanderen.Basisregisters.Api
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Autofac;
    using Be.Vlaanderen.Basisregisters.DataDog.Tracing.Autofac;
    using DataDog.Tracing;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;
    using Newtonsoft.Json;
    using Polly;
    using Serilog;
    using SqlStreamStore;

    public static class StartupHelpers
    {
        public const string AllowSpecificOrigin = "AllowSpecificOrigin";

        public static void RegisterApplicationLifetimeHandling(
            IContainer applicationContainer,
            IHostApplicationLifetime appLifetime,
            TraceAgent traceAgent)
        {
            appLifetime.ApplicationStarted.Register(() => Log.Information("Application started."));

            appLifetime.ApplicationStopping.Register(() =>
            {
                traceAgent?.OnCompleted();
                traceAgent?.Completion.Wait();

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

        [Obsolete("Please migrate to UseDataDog<T>(this IApplicationBuilder app, DataDogOptions options")]
        public static IApplicationBuilder UseDatadog<T>(
            this IApplicationBuilder app,
            IServiceProvider serviceProvider,
            ILoggerFactory loggerFactory,
            ApiDataDogToggle datadogToggle,
            ApiDebugDataDogToggle debugDataDogToggle,
            string serviceName,
            string traceIdHeaderName = DataDogOptions.DefaultTraceIdHeaderName,
            Func<StringValues, long> traceIdGenerator = null,
            Func<string, bool> shouldTracePath = null,
            string parentSpanIdHeaderName = DataDogOptions.DefaultParentSpanIdHeaderName)
        {
            return app.UseDataDog<T>(new DataDogOptions
            {
                Common =
                {
                    ServiceProvider = serviceProvider,
                    LoggerFactory = loggerFactory
                },
                Toggles =
                {
                    Enable = datadogToggle,
                    Debug = debugDataDogToggle
                },
                Tracing =
                {
                    ServiceName = serviceName,
                    TraceIdHeaderName = traceIdHeaderName,
                    ParentSpanIdHeaderName = parentSpanIdHeaderName,
                    TraceIdGenerator = traceIdGenerator,
                    ShouldTracePath = shouldTracePath
                }
            });
        }

        public static void SetupSourceListener(TraceSource source)
        {
            var serializer = new JsonSerializer { Formatting = Formatting.Indented };

            source.Subscribe(t =>
            {
                var sb = new StringBuilder("========== Begin Trace ==========");

                using (var writer = new StringWriter(sb))
                {
                    writer.WriteLine();
                    serializer.Serialize(writer, t);
                    writer.WriteLine("========== End Trace ==========");
                    writer.Flush();
                }

                Console.WriteLine(sb.ToString());
            });
        }

        public static async Task CheckDatabases(
            HealthCheckService healthCheckService,
            string databaseTag)
        {
            string FormatHealthReport(HealthReport healthReport)
            {
                var entries = healthReport
                    .Entries
                    .Where(x => x.Value.Status != HealthStatus.Healthy)
                    .Select(x => $"{x.Key} - {x.Value.Exception?.Message}");

                return $"\n\t* {string.Join("\n\t* ", entries)}";
            }

            var result = await healthCheckService.CheckHealthAsync(x => x.Tags.Contains(databaseTag));

            if (result.Status != HealthStatus.Healthy)
                throw new Exception($"Databases not ready:{FormatHealthReport(result)}");
        }
    }
}
