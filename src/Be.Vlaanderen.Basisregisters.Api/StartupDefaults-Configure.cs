namespace Be.Vlaanderen.Basisregisters.Api
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO.Compression;
    using System.Linq;
    using System.Net.Http;
    using Asp.Versioning.ApiExplorer;
    using Asp.Versioning.ApplicationModels;
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
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Localization;
    using Microsoft.Net.Http.Headers;
    using Microsoft.OpenApi.Models;
    using Search.Filtering;
    using Search.Pagination;
    using Search.Sorting;
    using SwaggerOptions = AspNetCore.Swagger.SwaggerOptions;

    public class StartupConfigureOptions
    {
        public bool EnableJsonErrorActionFilter { get; set; } = false;
        public StartupConfigureOptions EnableJsonErrorActionFilterOption()
        {
            EnableJsonErrorActionFilter = true;
            return this;
        }

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
            public string BaseUrl { get; set; } = string.Empty;

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

            public string ProblemDetailsTypeNamespaceOverride { get; set; } = string.Empty;
        }

        public SwaggerOptions Swagger { get; } = new SwaggerOptions();

        public class SwaggerOptions
        {
            /// <summary>
            /// Function which returns global metadata to be included in the Swagger output.
            /// </summary>
            public Func<IApiVersionDescriptionProvider, ApiVersionDescription, OpenApiInfo> ApiInfo { get; set; }

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
            public bool EnableFluentValidation { get; set; } = true;
            public Action<FluentValidationMvcConfiguration>? FluentValidation { get; set; }
            public Action<MvcDataAnnotationsLocalizationOptions> DataAnnotationsLocalization { get; set; }
            public Action<AuthorizationOptions> Authorization { get; set; }

            public Action<IMvcCoreBuilder>? AfterMvcCore { get; set; }
            public Action<IMvcCoreBuilder>? AfterMvc { get; set; }
            public Action<IHealthChecksBuilder>? AfterHealthChecks { get; set; }

            public Action<MvcOptions>? ConfigureMvcCore { get; set; }
            public Action<MvcNewtonsoftJsonOptions>? ConfigureJsonOptions { get; set; }
            public Action<FormatterMappings>? ConfigureFormatterMappings { get; set; }
            public Action<Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions>? ConfigureCors { get; set; }
            public Action<ProblemDetailsOptions>? ConfigureProblemDetails { get; set; }
        }

        public ICollection<IActionModelConvention> ActionModelConventions { get; } = new Collection<IActionModelConvention>();
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
            {
                throw new ArgumentNullException(nameof(options.Swagger.ApiInfo));
            }

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

            services.AddSingleton(options);

            services.TryAddEnumerable(ServiceDescriptor.Transient<IApiControllerSpecification, ApiControllerSpec>());

            services
                .AddHttpContextAccessor()

                .ConfigureOptions<ProblemDetailsSetup>()
                .AddProblemDetails(cfg =>
                {
                    foreach (var header in configuredCorsExposedHeaders)
                    {
                        if (!cfg.AllowedHeaderNames.Contains(header))
                        {
                            cfg.AllowedHeaderNames.Add(header);
                        }
                    }

                    options.MiddlewareHooks.ConfigureProblemDetails?.Invoke(cfg);
                });

            var mvcBuilder = services
                .AddMvcCore(cfg =>
                {
                    cfg.RespectBrowserAcceptHeader = false;
                    cfg.ReturnHttpNotAcceptable = true;

                    cfg.Filters.Add(new LoggingFilterFactory(options.Server.MethodsToLog));

                    // This got removed in .NET Core 3.0, we need to determine the impact
                    //cfg.Filters.Add(new CorsAuthorizationFilterFactory(StartupHelpers.AllowSpecificOrigin));

                    cfg.Filters.Add<OperationCancelledExceptionFilterAttribute>();

                    cfg.Filters.Add(new DataDogTracingFilter());

                    if (options.EnableJsonErrorActionFilter)
                    {
                        cfg.Filters.Add(new JsonErrorActionFilter());
                    }

                    foreach (var actionModelConvention in options.ActionModelConventions)
                    {
                        cfg.Conventions.Add(actionModelConvention);
                    }

                    options.MiddlewareHooks.ConfigureMvcCore?.Invoke(cfg);
                });

            options.MiddlewareHooks.AfterMvcCore?.Invoke(mvcBuilder);

            mvcBuilder
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddDataAnnotationsLocalization(options.MiddlewareHooks.DataAnnotationsLocalization);

            if(options.MiddlewareHooks.EnableFluentValidation)
            {
                mvcBuilder.AddFluentValidation(
                    options.MiddlewareHooks.FluentValidation
                    ?? (fv => fv.RegisterValidatorsFromAssemblyContaining<T>()));
            }

            mvcBuilder
                .AddCors(cfg =>
                {
                    cfg.AddPolicy(StartupHelpers.AllowAnyOrigin, corsPolicy => corsPolicy
                        .AllowAnyOrigin()
                        .WithMethods(configuredCorsMethods)
                        .WithHeaders(configuredCorsHeaders)
                        .WithExposedHeaders(configuredCorsExposedHeaders)
                        .SetPreflightMaxAge(TimeSpan.FromSeconds(60 * 15)));

                    cfg.AddPolicy(StartupHelpers.AllowSpecificOrigin, corsPolicy => corsPolicy
                        .WithOrigins(options.Cors.Origins ?? new string[0])
                        .WithMethods(configuredCorsMethods)
                        .WithHeaders(configuredCorsHeaders)
                        .WithExposedHeaders(configuredCorsExposedHeaders)
                        .SetPreflightMaxAge(TimeSpan.FromSeconds(60 * 15))
                        .AllowCredentials());

                    options.MiddlewareHooks.ConfigureCors?.Invoke(cfg);
                })

                .AddControllersAsServices()
                .AddAuthorization(options.MiddlewareHooks.Authorization)

                .AddNewtonsoftJson(
                    options.MiddlewareHooks.ConfigureJsonOptions
                    ?? (cfg =>
                    {
                        cfg.SerializerSettings.ConfigureDefaultForApi();
                        cfg.AllowInputFormatterExceptionMessages = !options.EnableJsonErrorActionFilter;
                    }))

                .AddXmlDataContractSerializerFormatters()

                .AddFormatterMappings(options.MiddlewareHooks.ConfigureFormatterMappings)

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


                .AddApiVersioning(cfg =>
                {
                    cfg.ReportApiVersions = true;
                })
                .AddApiExplorer(cfg =>
                {
                    cfg.GroupNameFormat = "'v'VVV";
                    cfg.SubstituteApiVersionInUrl = true;
                });

            services
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
                .Configure<BrotliCompressionProviderOptions>(cfg => cfg.Level = CompressionLevel.Fastest)
                .Configure<KestrelServerOptions>(serverOptions => serverOptions.AllowSynchronousIO = true);

            ValidatorOptions.Global.DisplayNameResolver =
                (type, member, expression) =>
                    member != null
                        ? GlobalStringLocalizer.Instance.GetLocalizer<TSharedResources>().GetString(() => member.Name)
                        : null;

            return services;
        }
    }
}
