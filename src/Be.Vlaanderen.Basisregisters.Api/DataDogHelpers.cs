namespace Be.Vlaanderen.Basisregisters.Api
{
    using System;
    using DataDog.Tracing;
    using DataDog.Tracing.AspNetCore;
    using DataDog.Tracing.Autofac;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;

    public class DataDogOptions
    {
        public const string DefaultTraceIdHeaderName = "X-Trace-Id";
        public const string DefaultParentSpanIdHeaderName = "X-Trace-Parent-Span-Id";

        public CommonOptions Common { get; } = new CommonOptions();

        public class CommonOptions
        {
            public IServiceProvider ServiceProvider { get; set; }
            public ILoggerFactory LoggerFactory { get; set; }
        }

        public ToggleOptions Toggles { get; } = new ToggleOptions();

        public class ToggleOptions
        {
            public ApiDataDogToggle Enable { get; set; }
            public ApiDebugDataDogToggle Debug { get; set; }
        }

        public TracingOptions Tracing { get; } = new TracingOptions();

        public class TracingOptions
        {
            public string ServiceName { get; set; }
            public string TraceIdHeaderName { get; set; } = DefaultTraceIdHeaderName;
            public string ParentSpanIdHeaderName { get; set; } = DefaultParentSpanIdHeaderName;
            public bool AnalyticsEnabled { get; set; }
            public bool LogForwardedForEnabled { get; set; }

            public Func<StringValues, long> TraceIdGenerator { get; set; }
            public Func<string, bool> ShouldTracePath { get; set; }
        }
    }

    public static class DataDogHelpers
    {
        private static readonly Random TraceIdGenerator = new Random();

        public static IApplicationBuilder UseDataDog<T>(
            this IApplicationBuilder app,
            DataDogOptions options)
        {
            if (options.Common.ServiceProvider == null)
            {
                throw new ArgumentNullException(nameof(options.Common.ServiceProvider));
            }

            if (options.Common.LoggerFactory == null)
            {
                throw new ArgumentNullException(nameof(options.Common.LoggerFactory));
            }

            if (options.Toggles.Enable == null)
            {
                throw new ArgumentNullException(nameof(options.Toggles.Enable));
            }

            if (options.Toggles.Debug == null)
            {
                throw new ArgumentNullException(nameof(options.Toggles.Debug));
            }

            if (string.IsNullOrWhiteSpace(options.Tracing.ServiceName))
            {
                throw new ArgumentNullException(nameof(options.Tracing.ServiceName));
            }

            if (string.IsNullOrWhiteSpace(options.Tracing.TraceIdHeaderName))
            {
                options.Tracing.TraceIdHeaderName = DataDogOptions.DefaultTraceIdHeaderName;
            }

            if (string.IsNullOrWhiteSpace(options.Tracing.ParentSpanIdHeaderName))
            {
                options.Tracing.ParentSpanIdHeaderName = DataDogOptions.DefaultParentSpanIdHeaderName;
            }

            if (options.Toggles.Enable.FeatureEnabled)
            {
                if (options.Toggles.Debug.FeatureEnabled)
                {
                    StartupHelpers.SetupSourceListener(options.Common.ServiceProvider.GetRequiredService<TraceSource>());
                }

                var traceSourceFactory = options.Common.ServiceProvider.GetRequiredService<Func<TraceSourceArguments, TraceSource>>();
                var logger = options.Common.LoggerFactory.CreateLogger<T>();

                app.UseDataDogTracing(new TraceOptions
                {
                    TraceSource = request =>
                    {
                        var traceId = request.ExtractTraceIdFromHeader(
                            options.Tracing.TraceIdHeaderName,
                            options.Tracing.TraceIdGenerator,
                            logger);

                        var traceParentSpanId = request.ExtractTraceParentSpanIdFromHeader(
                            options.Tracing.ParentSpanIdHeaderName,
                            logger);

                        return traceParentSpanId.HasValue
                            ? traceSourceFactory(new TraceSourceArguments(traceId, traceParentSpanId.Value))
                            : traceSourceFactory(new TraceSourceArguments(traceId));
                    },
                    ServiceName = options.Tracing.ServiceName,
                    ShouldTracePath = options.Tracing.ShouldTracePath ?? (
                        pathToCheck =>
                        {
                            if (string.IsNullOrWhiteSpace(pathToCheck))
                            {
                                pathToCheck = string.Empty;
                            }

                            pathToCheck = pathToCheck.ToLowerInvariant();

                            if (pathToCheck == "/")
                            {
                                return false;
                            }

                            if (pathToCheck == "/health")
                            {
                                return false;
                            }

                            if (pathToCheck.StartsWith("/docs"))
                            {
                                return false;
                            }

                            return true;
                        }),
                    AnalyticsEnabled = options.Tracing.AnalyticsEnabled,
                    LogForwardedForEnabled = options.Tracing.LogForwardedForEnabled
                });
            }

            return app;
        }

        private static long ExtractTraceIdFromHeader<T>(
            this HttpRequest request,
            string traceIdHeaderName,
            Func<StringValues, long> traceIdGenerator,
            ILogger<T> logger)
        {
            long traceId = TraceIdGenerator.Next(1, int.MaxValue);
            try
            {
                logger.LogDebug("Trying to parse Trace Id from {Headers}", request.Headers);

                if (request.Headers.TryGetValue(traceIdHeaderName, out var traceIdHeader))
                {
                    if (traceIdGenerator != null)
                    {
                        traceId = traceIdGenerator(traceIdHeader);
                    }
                    else
                    {
                        if (long.TryParse(traceIdHeader.ToString(), out var possibleTraceId))
                        {
                            traceId = possibleTraceId;
                        }
                    }
                }

                logger.LogDebug("Parsed {ParsedTraceId}", traceId);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to parse Trace Id from {Headers}.", request.Headers);
            }

            return traceId;
        }

        private static long? ExtractTraceParentSpanIdFromHeader<T>(
            this HttpRequest request,
            string parentSpanIdHeaderName,
            ILogger<T> logger)
        {
            long? parentSpanId = null;
            try
            {
                logger.LogDebug("Trying to parse Parent Span Id from {Headers}", request.Headers);

                if (request.Headers.TryGetValue(parentSpanIdHeaderName, out var parentSpanIdHeader)
                    && long.TryParse(parentSpanIdHeader.ToString(), out var possibleParentSpanId))
                {
                    parentSpanId = possibleParentSpanId;
                }

                logger.LogDebug("Parsed {ParsedParentSpanId}", parentSpanId);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to parse Parent Span Id from {Headers}.", request.Headers);
            }

            return parentSpanId;
        }
    }
}
