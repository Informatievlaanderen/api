namespace Be.Vlaanderen.Basisregisters.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Asp.Versioning.ApiExplorer;
    using AspNetCore.Mvc.Middleware;
    using AspNetCore.Swagger.ReDoc;
    using DataDog.Tracing;
    using Exceptions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Autofac;
    using BasicApiProblem;
    using Localization;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Options;
    using Middleware.AddProblemJsonHeader;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class StartupUseOptions
    {
        public CommonOptions Common { get; } = new CommonOptions();

        public class CommonOptions
        {
            public IContainer ApplicationContainer { get; set; }
            public IServiceProvider ServiceProvider { get; set; }
            public IWebHostEnvironment HostingEnvironment { get; set; }
            public IHostApplicationLifetime ApplicationLifetime { get; set; }
            public ILoggerFactory LoggerFactory { get; set; }
        }

        public ApiOptions Api { get; } = new ApiOptions();

        public class ApiOptions
        {
            /// <summary>
            /// Defines the behavior of a provider that discovers and describes API version information within an application.
            /// </summary>
            public IApiVersionDescriptionProvider VersionProvider { get; set; }

            /// <summary>
            /// Sets a title for the ReDoc page.
            /// This is used in the &lt;title&gt; tag.
            /// </summary>
            public Func<string, string> Info { get; set; }

            /// <summary>
            /// Sets a description for the ReDoc page.
            /// This is used in the &lt;meta name="description"&gt; tag.
            /// </summary>
            public Func<string, string> Description { get; set; }

            /// <summary>
            /// Sets an application name for the ReDoc page.
            /// This is used in the &lt;apple-mobile-web-app-title&gt; and &lt;application-name&gt; tag.
            /// </summary>
            public Func<string, string> ApplicationName { get; set; }

            /// <summary>
            /// Sets a header title for the ReDoc page.
            /// This is visible on the page.
            /// </summary>
            public Func<string, string> HeaderTitle { get; set; }

            /// <summary>
            /// Sets a header link for the ReDoc page.
            /// This is visible on the page in conjunction with HeaderTitle.
            /// </summary>
            public Func<string, string> HeaderLink { get; set; }

            /// <summary>
            /// Sets additional content to place in the head of the ReDoc page.
            /// This is used in the &lt;head&gt; tag.
            /// </summary>
            public Func<string, string> HeadContent { get; set; }

            /// <summary>
            /// Sets the version to display in the footer.
            /// This is visible on the page.
            /// </summary>
            public string FooterVersion { get; set; }

            public SwaggerDocumentationOptions.CSharpClientOptions CSharpClientOptions { get; } = new SwaggerDocumentationOptions.CSharpClientOptions();

            public SwaggerDocumentationOptions.TypeScriptClientOptions TypeScriptClientOptions { get; } = new SwaggerDocumentationOptions.TypeScriptClientOptions();

            public IEnumerable<ApiProblemDetailsExceptionMapping> ProblemDetailsExceptionMappers { get; set; } = new ApiProblemDetailsExceptionMapping[] { };

            public IEnumerable<IExceptionHandler> CustomExceptionHandlers { get; set; } = new IExceptionHandler[] { };

            public string RemoteIpAddressClaimName { get; set; } = AddRemoteIpAddressMiddleware.UrnBasisregistersVlaanderenIp;

            public string DefaultCorsPolicy { get; set; } = StartupHelpers.AllowSpecificOrigin;
        }

        public ServerOptions Server { get; } = new ServerOptions();

        public class ServerOptions
        {
            public string ServerName { get; set; }
            public string PoweredByName { get; set; }
            public string VersionHeaderName { get; set; } = AddVersionHeaderMiddleware.HeaderName;
            public FrameOptionsDirectives FrameOptionsDirective { get; set; } = FrameOptionsDirectives.Deny;
        }

        public MiddlewareHookOptions MiddlewareHooks { get; } = new MiddlewareHookOptions();

        public class MiddlewareHookOptions
        {
            public HealthCheckOptions HealthChecks { get; set; }

            public Action<IApplicationBuilder> AfterProblemDetails { get; set; }
            public Action<IApplicationBuilder> AfterCors { get; set; }
            public Action<IApplicationBuilder> AfterApiExceptionHandler { get; set; }
            public Action<IApplicationBuilder> AfterMiddleware { get; set; }
            public Action<IApplicationBuilder> AfterResponseCompression { get; set; }
            public Action<IApplicationBuilder> AfterMvc { get; set; }
            public Action<IApplicationBuilder> AfterSwagger { get; set; }
            public Action<IApplicationBuilder> AfterRequestLocalization { get; set; }
            public Action<IApplicationBuilder> AfterHealthChecks { get; set; }
        }
    }

    public static partial class StartupDefaults
    {
        public static IApplicationBuilder UseDefaultForApi(
            this IApplicationBuilder app,
            StartupUseOptions options)
        {
            if (options.Common.ApplicationContainer == null)
                throw new ArgumentNullException(nameof(options.Common.ApplicationContainer));

            if (options.Common.ServiceProvider == null)
                throw new ArgumentNullException(nameof(options.Common.ServiceProvider));

            if (options.Common.HostingEnvironment == null)
                throw new ArgumentNullException(nameof(options.Common.HostingEnvironment));

            if (options.Common.ApplicationLifetime == null)
                throw new ArgumentNullException(nameof(options.Common.ApplicationLifetime));

            if (options.Common.LoggerFactory == null)
                throw new ArgumentNullException(nameof(options.Common.LoggerFactory));

            if (string.IsNullOrWhiteSpace(options.Server.ServerName))
                options.Server.ServerName = "Vlaamse overheid";

            if (string.IsNullOrWhiteSpace(options.Server.PoweredByName))
                options.Server.PoweredByName = "Vlaamse overheid - Basisregisters Vlaanderen";

            if (options.Api.VersionProvider == null)
                throw new ArgumentNullException(nameof(options.Api.VersionProvider));

            GlobalStringLocalizer.Instance = new GlobalStringLocalizer(app.ApplicationServices.GetRequiredService<IServiceProvider>());

            if (options.Common.HostingEnvironment.IsDevelopment())
                app
                    .UseDeveloperExceptionPage()
                    .UseDatabaseErrorPage()
                    .UseBrowserLink();

            app.UseCors(policyName: options.Api.DefaultCorsPolicy);
            options.MiddlewareHooks.AfterCors?.Invoke(app);

            app.UseProblemDetails();
            options.MiddlewareHooks.AfterProblemDetails?.Invoke(app);

            app.UseApiExceptionHandler(
                options.Api.DefaultCorsPolicy,
                options);
            options.MiddlewareHooks.AfterApiExceptionHandler?.Invoke(app);

            app
                .UseMiddleware<EnableRequestRewindMiddleware>()

                // https://github.com/serilog/serilog-aspnetcore/issues/59
                .UseMiddleware<AddCorrelationIdToResponseMiddleware>()
                .UseMiddleware<AddCorrelationIdMiddleware>()
                .UseMiddleware<AddCorrelationIdToLogContextMiddleware>()

                .UseMiddleware<AddHttpSecurityHeadersMiddleware>(
                    options.Server.ServerName,
                    options.Server.PoweredByName,
                    options.Server.FrameOptionsDirective)

                .UseMiddleware<AddRemoteIpAddressMiddleware>(options.Api.RemoteIpAddressClaimName)

                .UseMiddleware<AddVersionHeaderMiddleware>(options.Server.VersionHeaderName)
                .UseMiddleware<AddProblemJsonHeaderMiddleware>();

            options.MiddlewareHooks.AfterMiddleware?.Invoke(app);

            app
                .UseMiddleware<DefaultResponseCompressionQualityMiddleware>(new Dictionary<string, double>
                {
                    { "br", 1.0 },
                    { "gzip", 0.9 }
                })
                .UseResponseCompression();
            options.MiddlewareHooks.AfterResponseCompression?.Invoke(app);

            var healthCheckOptions = new HealthCheckOptions
            {
                AllowCachingResponses = false,

                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                },

                ResponseWriter = (httpContext, healthReport) =>
                {
                    httpContext.Response.ContentType = "application/json";

                    var json = new JObject(
                        new JProperty("status", healthReport.Status.ToString()),
                        new JProperty("totalDuration", healthReport.TotalDuration.ToString()),
                        new JProperty("results", new JObject(healthReport.Entries.Select(pair =>
                            new JProperty(pair.Key, new JObject(
                                new JProperty("status", pair.Value.Status.ToString()),
                                new JProperty("duration", pair.Value.Duration),
                                new JProperty("description", pair.Value.Description),
                                new JProperty("exception", pair.Value.Exception?.Message),
                                new JProperty("data", new JObject(pair.Value.Data.Select(
                                    p => new JProperty(p.Key, p.Value))))))))));

                    return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
                }
            };

            app.UseHealthChecks("/health", options.MiddlewareHooks.HealthChecks ?? healthCheckOptions);
            options.MiddlewareHooks.AfterHealthChecks?.Invoke(app);

            var requestLocalizationOptions = options
                .Common
                .ServiceProvider
                .GetRequiredService<IOptions<RequestLocalizationOptions>>()
                .Value;

            app.UseRequestLocalization(requestLocalizationOptions);
            options.MiddlewareHooks.AfterRequestLocalization?.Invoke(app);

            app.UseSwaggerDocumentation(new SwaggerDocumentationOptions
            {
                ApiVersionDescriptionProvider = options.Api.VersionProvider,
                DocumentTitleFunc = options.Api.Info,
                DocumentDescriptionFunc = options.Api.Description,
                ApplicationNameFunc = options.Api.ApplicationName,
                HeaderTitleFunc = options.Api.HeaderTitle,
                HeaderLinkFunc = options.Api.HeaderLink,
                HeadContentFunc = options.Api.HeadContent,
                FooterVersion = options.Api.FooterVersion,
                CSharpClient =
                {
                    ClassName = options.Api.CSharpClientOptions.ClassName,
                    Namespace =  options.Api.CSharpClientOptions.Namespace
                },
                TypeScriptClient =
                {
                    ClassName = options.Api.TypeScriptClientOptions.ClassName
                }
            });
            options.MiddlewareHooks.AfterSwagger?.Invoke(app);

            UseRoutingAndEndpoints(app, options);

            StartupHelpers.RegisterApplicationLifetimeHandling(
                options.Common.ApplicationContainer,
                options.Common.ApplicationLifetime,
                options.Common.ServiceProvider.GetService<TraceAgent>());

            return app;
        }

        private static void UseRoutingAndEndpoints(IApplicationBuilder app, StartupUseOptions options)
        {
            // NOTE 2024-03-28:
            // Do not use UseMvc() instead of UseRouting()+UseEndpoints()
            // Since upgrade to dotnet8, that breaks ApiVersioning via attribute routing

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            options.MiddlewareHooks.AfterMvc?.Invoke(app);
        }
    }
}
