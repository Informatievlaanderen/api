namespace Be.Vlaanderen.Basisregisters.Api
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
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
    using FluentValidation.AspNetCore;
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
            string[] corsOrigins = null,
            string[] corsMethods = null,
            string[] corsHeaders = null,
            string[] corsExposedHeaders = null,
            Action<FluentValidationMvcConfiguration> configureFluentValidation = null,
            Action<IMvcCoreBuilder> configureMvcBuilder = null)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IApiControllerSpecification, ApiControllerSpec>());

            var configuredCorsMethods = new[]
            {
                HttpMethod.Get.Method,
                HttpMethod.Head.Method,
                HttpMethod.Post.Method,
                HttpMethod.Put.Method,
                HttpMethod.Patch.Method,
                HttpMethod.Delete.Method
            }.Union(corsMethods ?? new string[] {}).Distinct().ToArray();

            var configuredCorsHeaders = new[]
            {
                HeaderNames.Accept,
                HeaderNames.ContentType,
                HeaderNames.Origin,
                HeaderNames.Authorization,
                HeaderNames.IfMatch,
                ExtractFilteringRequestExtension.HeaderName,
                AddSortingExtension.HeaderName,
                AddPaginationExtension.HeaderName
            }.Union(corsHeaders ?? new string[] { }).Distinct().ToArray();

            var configuredCorsExposedHeaders = new[]
            {
                HeaderNames.Location,
                ExtractFilteringRequestExtension.HeaderName,
                AddSortingExtension.HeaderName,
                AddPaginationExtension.HeaderName,
                AddVersionHeaderMiddleware.HeaderName,
                AddCorrelationIdToResponseMiddleware.HeaderName,
                AddHttpSecurityHeadersMiddleware.PoweredByHeaderName,
                AddHttpSecurityHeadersMiddleware.ContentTypeOptionsHeaderName,
                AddHttpSecurityHeadersMiddleware.FrameOptionsHeaderName,
                AddHttpSecurityHeadersMiddleware.XssProtectionHeaderName
            }.Union(corsExposedHeaders ?? new string[] { }).Distinct().ToArray();

            var mvcBuilder = services
                .AddMvcCore(options =>
                {
                    options.RespectBrowserAcceptHeader = false;
                    options.ReturnHttpNotAcceptable = true;

                    options.Filters.Add(new LoggingFilterFactory());
                    options.Filters.Add(new CorsAuthorizationFilterFactory(StartupHelpers.AllowSpecificOrigin));
                    options.Filters.Add<OperationCancelledExceptionFilter>();

                    options.Filters.Add(new DataDogTracingFilter());
                })
                .AddFluentValidation(configureFluentValidation ?? (fv => fv.RegisterValidatorsFromAssemblyContaining<T>()))

                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)

                .AddCors(options => options.AddPolicy(StartupHelpers.AllowSpecificOrigin, corsPolicy => corsPolicy
                    .WithOrigins(corsOrigins ?? new string[] { })
                    .WithMethods(configuredCorsMethods)
                    .WithHeaders(configuredCorsHeaders)
                    .WithExposedHeaders(configuredCorsExposedHeaders)
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(60 * 15))
                    .AllowCredentials()))

                .AddControllersAsServices()
                .AddAuthorization()

                .AddJsonFormatters()
                .AddJsonOptions(options => options.SerializerSettings.ConfigureDefaultForApi())

                .AddXmlDataContractSerializerFormatters()

                .AddApiExplorer();

            configureMvcBuilder?.Invoke(mvcBuilder);

            mvcBuilder
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

        [Obsolete("Use UseDefaultForApi(<StartupOptions>) instead.")]
        public static IApplicationBuilder UseDefaultForApi(
            this IApplicationBuilder app,
            IContainer applicationContainer,
            IServiceProvider serviceProvider,
            IHostingEnvironment env,
            IApplicationLifetime appLifetime,
            ILoggerFactory loggerFactory,
            IApiVersionDescriptionProvider apiVersionProvider,
            Func<string, string> apiInfo,
            string serverName = "Vlaamse overheid",
            string poweredByName = "Vlaamse overheid - Basisregisters Vlaanderen",
            Action<IApplicationBuilder> afterCors = null,
            Action<IApplicationBuilder> afterApiExceptionHandler = null,
            Action<IApplicationBuilder> afterMiddleware = null,
            Action<IApplicationBuilder> afterResponseCompression = null,
            Action<IApplicationBuilder> afterMvc = null,
            Action<IApplicationBuilder> afterSwagger = null)
            => app.UseDefaultForApi(new StartupOptions
            {
                ApplicationContainer = applicationContainer,
                ServiceProvider = serviceProvider,
                HostingEnvironment = env,
                ApplicationLifetime = appLifetime,
                LoggerFactory = loggerFactory,
                Api =
                {
                    VersionProvider = apiVersionProvider,
                    Info = apiInfo
                },
                Server =
                {
                    ServerName = serverName,
                    PoweredByName = poweredByName
                },
                MiddlewareHooks =
                {
                    AfterCors = afterCors,
                    AfterApiExceptionHandler = afterApiExceptionHandler,
                    AfterMiddleware = afterMiddleware,
                    AfterResponseCompression = afterResponseCompression,
                    AfterMvc = afterMvc,
                    AfterSwagger = afterSwagger
                }
            });

        public static IApplicationBuilder UseDefaultForApi(
            this IApplicationBuilder app,
            StartupOptions options)
        {
            if (options.ApplicationContainer == null)
                throw new ArgumentNullException(nameof(options.ApplicationContainer));

            if (options.ServiceProvider == null)
                throw new ArgumentNullException(nameof(options.ServiceProvider));

            if (options.HostingEnvironment == null)
                throw new ArgumentNullException(nameof(options.HostingEnvironment));

            if (options.ApplicationLifetime == null)
                throw new ArgumentNullException(nameof(options.ApplicationLifetime));

            if (options.LoggerFactory == null)
                throw new ArgumentNullException(nameof(options.LoggerFactory));

            if (string.IsNullOrWhiteSpace(options.Server.ServerName))
                options.Server.ServerName = "Vlaamse overheid";

            if (string.IsNullOrWhiteSpace(options.Server.PoweredByName))
                options.Server.PoweredByName = "Vlaamse overheid - Basisregisters Vlaanderen";

            if (options.Api.VersionProvider == null)
                throw new ArgumentNullException(nameof(options.Api.VersionProvider));

            if (options.Api.Info == null)
                throw new ArgumentNullException(nameof(options.Api.Info));

            if (options.HostingEnvironment.IsDevelopment())
                app
                    .UseDeveloperExceptionPage()
                    .UseDatabaseErrorPage()
                    .UseBrowserLink();

            app.UseCors(policyName: StartupHelpers.AllowSpecificOrigin);
            options.MiddlewareHooks.AfterCors?.Invoke(app);

            app.UseApiExceptionHandler(
                options.LoggerFactory,
                StartupHelpers.AllowSpecificOrigin,
                options.Api.CustomExceptionHandlers);
            options.MiddlewareHooks.AfterApiExceptionHandler?.Invoke(app);

            app
                .UseMiddleware<EnableRequestRewindMiddleware>()

                // https://github.com/serilog/serilog-aspnetcore/issues/59
                .UseMiddleware<AddCorrelationIdToResponseMiddleware>()
                .UseMiddleware<AddCorrelationIdMiddleware>()
                .UseMiddleware<AddCorrelationIdToLogContextMiddleware>()

                .UseMiddleware<AddHttpSecurityHeadersMiddleware>(options.Server.ServerName, options.Server.PoweredByName)

                .UseMiddleware<AddRemoteIpAddressMiddleware>()

                .UseMiddleware<AddVersionHeaderMiddleware>();
            options.MiddlewareHooks.AfterMiddleware?.Invoke(app);

            app
                .UseMiddleware<DefaultResponseCompressionQualityMiddleware>(new Dictionary<string, double>
                {
                    { "br", 1.0 },
                    { "gzip", 0.9 }
                })
                .UseResponseCompression();
            options.MiddlewareHooks.AfterResponseCompression?.Invoke(app);

            app.UseSwaggerDocumentation(options.Api.VersionProvider, options.Api.Info);
            options.MiddlewareHooks.AfterSwagger?.Invoke(app);

            app.UseMvc();
            options.MiddlewareHooks.AfterMvc?.Invoke(app);

            StartupHelpers.RegisterApplicationLifetimeHandling(
                options.ApplicationContainer,
                options.ApplicationLifetime,
                options.ServiceProvider.GetService<TraceAgent>());

            return app;
        }
    }

    public class StartupOptions
    {
        public IContainer ApplicationContainer { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        public IHostingEnvironment HostingEnvironment { get; set; }
        public IApplicationLifetime ApplicationLifetime { get; set; }
        public ILoggerFactory LoggerFactory { get; set; }

        public ApiOptions Api { get; } = new ApiOptions();

        public class ApiOptions
        {
            public IApiVersionDescriptionProvider VersionProvider { get; set; }
            public Func<string, string> Info { get; set; }
            public IEnumerable<IExceptionHandler> CustomExceptionHandlers { get; set; } = new IExceptionHandler[] { };
        }

        public ServerOptions Server { get; } = new ServerOptions();

        public class ServerOptions
        {
            public string ServerName { get; set; }
            public string PoweredByName { get; set; }
        }

        public MiddlewareHookOptions MiddlewareHooks { get; } = new MiddlewareHookOptions();

        public class MiddlewareHookOptions
        {
            public Action<IApplicationBuilder> AfterCors { get; set; }
            public Action<IApplicationBuilder> AfterApiExceptionHandler { get; set; }
            public Action<IApplicationBuilder> AfterMiddleware { get; set; }
            public Action<IApplicationBuilder> AfterResponseCompression { get; set; }
            public Action<IApplicationBuilder> AfterMvc { get; set; }
            public Action<IApplicationBuilder> AfterSwagger { get; set; }
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
