namespace Be.Vlaanderen.Basisregisters.Api
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.IO.Compression;
    using System.Net.Http;
    using System.Text;
    using AspNetCore.Mvc.Formatters.Json;
    using AspNetCore.Mvc.Logging;
    using AspNetCore.Mvc.Middleware;
    using AspNetCore.Swagger;
    using AspNetCore.Swagger.ReDoc;
    using DataDog.Tracing;
    using DataDog.Tracing.AspNetCore;
    using Exceptions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Cors.Internal;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Polly;
    using Serilog;
    using SqlStreamStore;
    using Autofac;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Net.Http.Headers;
    using Search.Filtering;
    using Search.Pagination;
    using Search.Sorting;
    using Swashbuckle.AspNetCore.Swagger;

    public static class StartupDefaults
    {
        public static IServiceCollection ConfigureDefaultForApi<T>(
            this IServiceCollection services,
            Func<IApiVersionDescriptionProvider, ApiVersionDescription, Info> apiInfo,
            string[] xmlCommentPaths,
            string[] corsOrigins)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IApiControllerSpecification, ApiControllerSpec>());

            services
                .AddMvcCore(options =>
                {
                    options.RespectBrowserAcceptHeader = false;
                    options.ReturnHttpNotAcceptable = true;

                    options.Filters.Add(new LoggingFilterFactory());
                    options.Filters.Add(new CorsAuthorizationFilterFactory(StartupHelpers.AllowSpecificOrigin));
                    options.Filters.Add<OperationCancelledExceptionFilter>();

                    options.Filters.Add(new DataDogTracingFilter());
                })

                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)

                .AddCors(options => options.AddPolicy(StartupHelpers.AllowSpecificOrigin, corsPolicy => corsPolicy
                    .WithOrigins(corsOrigins)
                    .WithMethods(
                        HttpMethod.Get.Method,
                        HttpMethod.Head.Method,
                        HttpMethod.Post.Method,
                        HttpMethod.Put.Method,
                        HttpMethod.Delete.Method)
                    .WithHeaders(
                        HeaderNames.Accept,
                        HeaderNames.ContentType,
                        HeaderNames.Origin,
                        HeaderNames.Authorization,
                        HeaderNames.IfMatch,
                        ExtractFilteringRequestExtension.HeaderName,
                        AddSortingExtension.HeaderName,
                        AddPaginationExtension.HeaderName)
                    .WithExposedHeaders(
                        HeaderNames.Location,
                        ExtractFilteringRequestExtension.HeaderName,
                        AddSortingExtension.HeaderName,
                        AddPaginationExtension.HeaderName,
                        AddVersionHeaderMiddleware.HeaderName,
                        AddCorrelationIdToResponseMiddleware.HeaderName,
                        AddHttpSecurityHeadersMiddleware.PoweredByHeaderName,
                        AddHttpSecurityHeadersMiddleware.ContentTypeOptionsHeaderName,
                        AddHttpSecurityHeadersMiddleware.FrameOptionsHeaderName,
                        AddHttpSecurityHeadersMiddleware.XssProtectionHeaderName)
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(60 * 15))
                    .AllowCredentials()))

                .AddControllersAsServices()
                .AddAuthorization()

                .AddJsonFormatters()
                .AddJsonOptions(options => options.SerializerSettings.ConfigureDefaultForApi())

                .AddXmlDataContractSerializerFormatters()

                .AddApiExplorer()
                .Services

                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                })

                .AddApiVersioning(x => x.ReportApiVersions = true)

                .AddSwagger<T>(apiInfo, xmlCommentPaths)

                .AddResponseCompression(options =>
                {
                    options.EnableForHttps = true;

                    options.Providers.Add<GzipCompressionProvider>();

                    options.MimeTypes = new[]
                    {
                        // General
                        "text/plain",
                        "text/csv",

                        // Static files
                        "text/css",
                        "application/javascript",

                        // MVC
                        "text/html",
                        "application/xml",
                        "text/xml",
                        "application/json",
                        "text/json",
                        "application/ld+json",
                        "application/atom+xml",

                        // Fonts
                        "application/font-woff",
                        "font/otf",
                        "application/vnd.ms-fontobject"
                    };
                })
                .Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            return services;
        }

        public static IApplicationBuilder UseDefaultForApi(
            this IApplicationBuilder app,
            IContainer applicationContainer,
            IServiceProvider serviceProvider,
            IHostingEnvironment env,
            IApplicationLifetime appLifetime,
            ILoggerFactory loggerFactory,
            IApiVersionDescriptionProvider apiVersionProvider,
            Func<string, string> apiInfo)
        {
            if (env.IsDevelopment())
                app
                    .UseDeveloperExceptionPage()
                    .UseDatabaseErrorPage()
                    .UseBrowserLink();

            app
                .UseCors(policyName: StartupHelpers.AllowSpecificOrigin)

                .UseApiExceptionHandler(loggerFactory, StartupHelpers.AllowSpecificOrigin)

                .UseMiddleware<EnableRequestRewindMiddleware>()

                // https://github.com/serilog/serilog-aspnetcore/issues/59
                .UseMiddleware<AddCorrelationIdToResponseMiddleware>()
                .UseMiddleware<AddCorrelationIdMiddleware>()
                .UseMiddleware<AddCorrelationIdToLogContextMiddleware>()

                .UseMiddleware<AddHttpSecurityHeadersMiddleware>()
                .UseMiddleware<AddRemoteIpAddressMiddleware>()
                .UseMiddleware<AddVersionHeaderMiddleware>()

                .UseMiddleware<DefaultResponseCompressionQualityMiddleware>(new Dictionary<string, double>
                {
                    { "br", 1.0 },
                    { "gzip", 0.9 }
                })
                .UseResponseCompression()

                .UseMvc()

                .UseSwaggerDocumentation(apiVersionProvider, apiInfo);

            StartupHelpers.RegisterApplicationLifetimeHandling(
                applicationContainer,
                appLifetime,
                serviceProvider.GetService<TraceAgent>());

            return app;
        }
    }

    public static class StartupHelpers
    {
        public const string AllowSpecificOrigin = "AllowSpecificOrigin";

        public static void RegisterApplicationLifetimeHandling(
            IContainer applicationContainer,
            IApplicationLifetime appLifetime,
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

        public static void EnsureSqlStreamStoreSchema<T>(MsSqlStreamStore streamStore, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<T>();

            // TODO: Need to revisit this with a Consul lock
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
    }
}
