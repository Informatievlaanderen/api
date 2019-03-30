namespace Be.Vlaanderen.Basisregisters.Api
{
    using System;
    using System.Collections.Generic;
    using AspNetCore.Mvc.Middleware;
    using AspNetCore.Swagger.ReDoc;
    using DataDog.Tracing;
    using Exceptions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Autofac;
    using Localization;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;

    public class StartupUseOptions
    {
        public CommonOptions Common { get; } = new CommonOptions();

        public class CommonOptions
        {
            public IContainer ApplicationContainer { get; set; }
            public IServiceProvider ServiceProvider { get; set; }
            public IHostingEnvironment HostingEnvironment { get; set; }
            public IApplicationLifetime ApplicationLifetime { get; set; }
            public ILoggerFactory LoggerFactory { get; set; }
        }

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
            public Action<IApplicationBuilder> AfterRequestLocalization { get; set; }
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

            if (options.Api.Info == null)
                throw new ArgumentNullException(nameof(options.Api.Info));

            if (options.Common.HostingEnvironment.IsDevelopment())
                app
                    .UseDeveloperExceptionPage()
                    .UseDatabaseErrorPage()
                    .UseBrowserLink();

            app.UseCors(policyName: StartupHelpers.AllowSpecificOrigin);
            options.MiddlewareHooks.AfterCors?.Invoke(app);

            app.UseApiExceptionHandler(
                options.Common.LoggerFactory,
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

            app.UseRequestLocalization(options.Common.ServiceProvider.GetRequiredService<RequestLocalizationOptions>());
            options.MiddlewareHooks.AfterRequestLocalization?.Invoke(app);

            app.UseSwaggerDocumentation(options.Api.VersionProvider, options.Api.Info);
            options.MiddlewareHooks.AfterSwagger?.Invoke(app);

            app.UseMvc();
            options.MiddlewareHooks.AfterMvc?.Invoke(app);

            GlobalStringLocalizer.Instance = new GlobalStringLocalizer(app.ApplicationServices.GetRequiredService<IServiceProvider>());

            StartupHelpers.RegisterApplicationLifetimeHandling(
                options.Common.ApplicationContainer,
                options.Common.ApplicationLifetime,
                options.Common.ServiceProvider.GetService<TraceAgent>());

            return app;
        }
    }
}
