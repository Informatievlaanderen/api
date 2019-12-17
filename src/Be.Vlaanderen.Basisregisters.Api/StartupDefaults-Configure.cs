namespace Be.Vlaanderen.Basisregisters.Api
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO.Compression;
    using System.Linq;
    using System.Net.Http;
    using AspNetCore.Mvc.Formatters.Json;
    using AspNetCore.Mvc.Logging;
    using AspNetCore.Mvc.Middleware;
    using AspNetCore.Swagger;
    using BasicApiProblem;
    using DataDog.Tracing.AspNetCore;
    using Exceptions;
    using FluentValidation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.Extensions.DependencyInjection;
    using FluentValidation.AspNetCore;
    using Localization;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.AspNetCore.Mvc.DataAnnotations;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Localization;
    using Microsoft.Net.Http.Headers;
    using Search.Filtering;
    using Search.Pagination;
    using Search.Sorting;
    using Swashbuckle.AspNetCore.Swagger;
    using SwaggerOptions = AspNetCore.Swagger.SwaggerOptions;

    public class StartupConfigureOptions
    {
        public CorsOptions Cors { get; } = new CorsOptions();

        public class CorsOptions
        {
            public string[] Origins { get; set; } = null;
            public string[] Methods { get; set; } = null;
            public string[] Headers { get; set; } = null;
            public string[] ExposedHeaders { get; set; } = null;
        }

        public ServerOptions Server { get; } = new ServerOptions();

        public class ServerOptions
        {
            public string VersionHeaderName { get; set; } = AddVersionHeaderMiddleware.HeaderName;
            public string[] MethodsToLog { get; set; } = new []
            {
                HttpMethod.Get,
                HttpMethod.Head,
                HttpMethod.Post,
                HttpMethod.Put,
                HttpMethod.Patch,
                HttpMethod.Delete,
                HttpMethod.Options
            }.Select(x => x.Method).ToArray();
        }

        public SwaggerOptions Swagger { get; } = new SwaggerOptions();

        public class SwaggerOptions
        {
            /// <summary>
            /// Function which returns global metadata to be included in the Swagger output.
            /// </summary>
            public Func<IApiVersionDescriptionProvider, ApiVersionDescription, Info> ApiInfo { get; set; }

            /// <summary>
            /// Inject human-friendly descriptions for Operations, Parameters and Schemas based on XML Comment files.
            /// A list of absolute paths to the files that contains XML Comments.
            /// </summary>
            public string[] XmlCommentPaths { get; set; } = null;

            /// <summary>
            /// Easily add additional header parameters to each request.
            /// </summary>
            public IEnumerable<HeaderOperationFilter> AdditionalHeaderOperationFilters { get; set; } = new List<HeaderOperationFilter>();

            /// <summary>
            /// Available servers.
            /// </summary>
            public IEnumerable<Server> Servers { get; set; } = new List<Server>();

            public Func<ApiDescription, string> CustomSortFunc { get; set; } = SortByTag.Sort;

            public MiddlewareHookOptions MiddlewareHooks { get; } = new MiddlewareHookOptions();

            public class MiddlewareHookOptions
            {
                public Action<Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions>? AfterSwaggerGen { get; set; }
            }
        }

        public LocalizationOptions Localization { get; } = new LocalizationOptions();

        public class LocalizationOptions
        {
            public CultureInfo DefaultCulture { get; set; } = null;
            public CultureInfo[] SupportedCultures { get; set; } = null;
        }

        public MiddlewareHookOptions MiddlewareHooks { get; } = new MiddlewareHookOptions();

        public class MiddlewareHookOptions
        {
            public Action<FluentValidationMvcConfiguration>? FluentValidation { get; set; }
            public Action<MvcDataAnnotationsLocalizationOptions> DataAnnotationsLocalization { get; set; }
            public Action<AuthorizationOptions> Authorization { get; set; }

            public Action<IMvcCoreBuilder>? AfterMvc { get; set; }
            public Action<IHealthChecksBuilder>? AfterHealthChecks { get; set; }

            public Action<MvcOptions>? ConfigureMvcCore { get; set; }
            public Action<MvcNewtonsoftJsonOptions>? ConfigureJsonOptions { get; set; }
        }
    }

    public static partial class StartupDefaults
    {
        public class DefaultResources { }

        public static IServiceCollection ConfigureDefaultForApi<T>(
            this IServiceCollection services,
            StartupConfigureOptions options) =>
            services.ConfigureDefaultForApi<T, DefaultResources>(options);

        public static IServiceCollection ConfigureDefaultForApi<T, TSharedResources>(
            this IServiceCollection services,
            StartupConfigureOptions options)
        {
            if (options.Swagger.ApiInfo == null)
                throw new ArgumentNullException(nameof(options.Swagger.ApiInfo));

            var configuredCorsMethods = new[]
            {
                HttpMethod.Get.Method,
                HttpMethod.Head.Method,
                HttpMethod.Post.Method,
                HttpMethod.Put.Method,
                HttpMethod.Patch.Method,
                HttpMethod.Delete.Method,
                HttpMethod.Options.Method
            }.Union(options.Cors.Methods ?? Array.Empty<string>()).Distinct().ToArray();

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
            }.Union(options.Cors.Headers ?? Array.Empty<string>()).Distinct().ToArray();

            var configuredCorsExposedHeaders = new[]
            {
                HeaderNames.Location,
                ExtractFilteringRequestExtension.HeaderName,
                AddSortingExtension.HeaderName,
                AddPaginationExtension.HeaderName,
                options.Server.VersionHeaderName,
                AddCorrelationIdToResponseMiddleware.HeaderName,
                AddHttpSecurityHeadersMiddleware.PoweredByHeaderName,
                AddHttpSecurityHeadersMiddleware.ContentTypeOptionsHeaderName,
                AddHttpSecurityHeadersMiddleware.FrameOptionsHeaderName,
                AddHttpSecurityHeadersMiddleware.XssProtectionHeaderName,
                AddVersionHeaderMiddleware.HeaderName
            }.Union(options.Cors.ExposedHeaders ?? Array.Empty<string>()).Distinct().ToArray();

            services.TryAddEnumerable(ServiceDescriptor.Transient<IApiControllerSpecification, ApiControllerSpec>());

            services
                .AddHttpContextAccessor()

                .ConfigureOptions<ProblemDetailsSetup>()
                .AddProblemDetails(x =>
                {
                    foreach (var header in configuredCorsExposedHeaders)
                    {
                        if (!x.AllowedHeaderNames.Contains(header))
                            x.AllowedHeaderNames.Add(header);
                    }
                });

            var mvcBuilder = services
                .AddMvcCore(cfg =>
                {
                    cfg.RespectBrowserAcceptHeader = false;
                    cfg.ReturnHttpNotAcceptable = true;

                    cfg.Filters.Add(new LoggingFilterFactory(options.Server.MethodsToLog));

                    // This got removed in .NET Core 3.0, we need to determine the impact
                    //cfg.Filters.Add(new CorsAuthorizationFilterFactory(StartupHelpers.AllowSpecificOrigin));

                    cfg.Filters.Add<OperationCancelledExceptionFilter>();

                    cfg.Filters.Add(new DataDogTracingFilter());

                    options.MiddlewareHooks.ConfigureMvcCore?.Invoke(cfg);
                })

                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)

                .AddDataAnnotationsLocalization(options.MiddlewareHooks.DataAnnotationsLocalization)

                .AddFluentValidation(
                    options.MiddlewareHooks.FluentValidation
                    ?? (fv => fv.RegisterValidatorsFromAssemblyContaining<T>()))

                .AddCors(cfg => cfg.AddPolicy(StartupHelpers.AllowSpecificOrigin, corsPolicy => corsPolicy
                    .WithOrigins(options.Cors.Origins ?? new string[0])
                    .WithMethods(configuredCorsMethods)
                    .WithHeaders(configuredCorsHeaders)
                    .WithExposedHeaders(configuredCorsExposedHeaders)
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(60 * 15))
                    .AllowCredentials()))

                .AddControllersAsServices()
                .AddAuthorization(options.MiddlewareHooks.Authorization)

                .AddNewtonsoftJson(
                    options.MiddlewareHooks.ConfigureJsonOptions
                    ?? (cfg => cfg.SerializerSettings.ConfigureDefaultForApi()))

                .AddXmlDataContractSerializerFormatters()

                .AddApiExplorer();

            options.MiddlewareHooks.AfterMvc?.Invoke(mvcBuilder);

            var healthChecksBuilder = services.AddHealthChecks();
            options.MiddlewareHooks.AfterHealthChecks?.Invoke(healthChecksBuilder);

            services
                .AddLocalization(cfg => cfg.ResourcesPath = "Resources")
                .AddSingleton<IStringLocalizerFactory, SharedStringLocalizerFactory<TSharedResources>>()
                .AddSingleton<ResourceManagerStringLocalizerFactory, ResourceManagerStringLocalizerFactory>()

                .Configure<RequestLocalizationOptions>(opts =>
                {
                    const string fallbackCulture = "en-GB";
                    var defaultRequestCulture = new RequestCulture(options.Localization.DefaultCulture ?? new CultureInfo(fallbackCulture));
                    var supportedCulturesOrDefault = options.Localization.SupportedCultures ?? new[] { new CultureInfo(fallbackCulture) };

                    opts.DefaultRequestCulture = defaultRequestCulture;
                    opts.SupportedCultures = supportedCulturesOrDefault;
                    opts.SupportedUICultures = supportedCulturesOrDefault;

                    opts.FallBackToParentCultures = true;
                    opts.FallBackToParentUICultures = true;
                })

                .AddVersionedApiExplorer(cfg =>
                {
                    cfg.GroupNameFormat = "'v'VVV";
                    cfg.SubstituteApiVersionInUrl = true;
                })

                .AddApiVersioning(cfg =>
                {
                    cfg.ReportApiVersions = true;
                    cfg.ErrorResponses = new ProblemDetailsResponseProvider();
                })

                .AddSwagger<T>(new SwaggerOptions
                {
                    ApiInfoFunc = options.Swagger.ApiInfo,
                    XmlCommentPaths = options.Swagger.XmlCommentPaths ?? new string[0],
                    AdditionalHeaderOperationFilters = options.Swagger.AdditionalHeaderOperationFilters,
                    Servers = options.Swagger.Servers,
                    CustomSortFunc = options.Swagger.CustomSortFunc,
                    MiddlewareHooks =
                    {
                        AfterSwaggerGen = options.Swagger.MiddlewareHooks.AfterSwaggerGen
                    }
                })

                .AddResponseCompression(cfg =>
                {
                    cfg.EnableForHttps = true;

                    cfg.Providers.Add<BrotliCompressionProvider>();
                    cfg.Providers.Add<GzipCompressionProvider>();

                    cfg.MimeTypes = new[]
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
                .Configure<GzipCompressionProviderOptions>(cfg => cfg.Level = CompressionLevel.Fastest)
                .Configure<BrotliCompressionProviderOptions>(cfg => cfg.Level = CompressionLevel.Fastest);

            ValidatorOptions.DisplayNameResolver =
                (type, member, expression) =>
                    member != null
                        ? GlobalStringLocalizer.Instance.GetLocalizer<TSharedResources>().GetString(() => member.Name)
                        : null;

            return services;
        }
    }
}
