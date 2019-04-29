namespace Dummy.Api.Infrastructure
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Be.Vlaanderen.Basisregisters.Api;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Configuration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Modules;
    using Swashbuckle.AspNetCore.Swagger;

    /// <summary>Represents the startup process for the application.</summary>
    public class Startup
    {
        private const string DefaultCulture = "en-GB";
        private const string SupportedCultures = "en-GB;en-US;en;nl-BE;nl;fr-BE;fr";

        private IContainer _applicationContainer;

        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        /// <summary>Configures services for the application.</summary>
        /// <param name="services">The collection of services to configure the application with.</param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .ConfigureDefaultForApi<Startup>(new StartupConfigureOptions
                {
                    Cors =
                    {
                        Origins = _configuration
                            .GetSection("Cors")
                            .GetChildren()
                            .Select(c => c.Value)
                            .ToArray()
                    },
                    Swagger =
                    {
                        ApiInfo = (provider, description) => new Info
                        {
                            Version = description.ApiVersion.ToString(),
                            Title = "Dummy API",
                            Description = description.GroupName,
                            Contact = new Contact
                            {
                                Name = "agentschap Informatie Vlaanderen",
                                Email = "informatie.vlaanderen@vlaanderen.be",
                                Url = "https://vlaanderen.be/informatie-vlaanderen"
                            }
                        },
                        XmlCommentPaths = new [] { typeof(Startup).GetTypeInfo().Assembly.GetName().Name }
                    },
                    Localization =
                    {
                        DefaultCulture = new CultureInfo(DefaultCulture),
                        SupportedCultures = SupportedCultures
                            .Split(';', StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => new CultureInfo(x.Trim()))
                            .ToArray()
                    },
                    MiddlewareHooks =
                    {
                        FluentValidation = fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>(),
                    }
                });

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new ApiModule(_configuration, services));
            _applicationContainer = containerBuilder.Build();

            return new AutofacServiceProvider(_applicationContainer);
        }

        public void Configure(
            IServiceProvider serviceProvider,
            IApplicationBuilder app,
            IHostingEnvironment env,
            IApplicationLifetime appLifetime,
            ILoggerFactory loggerFactory,
            IApiVersionDescriptionProvider apiVersionProvider)
        {
            app.UseDefaultForApi(new StartupUseOptions
            {
                Common =
                {
                    ApplicationContainer = _applicationContainer,
                    ServiceProvider = serviceProvider,
                    HostingEnvironment = env,
                    ApplicationLifetime = appLifetime,
                    LoggerFactory = loggerFactory,
                },
                Api =
                {
                    VersionProvider = apiVersionProvider,
                    Info = groupName => $"Dummy API {groupName}",
                },
                Server =
                {
                    PoweredByName = "Vlaamse overheid - Basisregisters Vlaanderen",
                    ServerName = "agentschap Informatie Vlaanderen"
                },
                MiddlewareHooks =
                {
                    AfterMiddleware = x => x.UseMiddleware<AddNoCacheHeadersMiddleware>()
                }
            });
        }
    }
}
