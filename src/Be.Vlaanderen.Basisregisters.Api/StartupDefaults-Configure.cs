namespace Be.Vlaanderen.Basisregisters.Api
{
    using System;
    using System.Globalization;
    using System.IO.Compression;
    using System.Linq;
    using System.Net.Http;
    using AspNetCore.Mvc.Formatters.Json;
    using AspNetCore.Mvc.Logging;
    using AspNetCore.Mvc.Middleware;
    using AspNetCore.Swagger;
    using DataDog.Tracing.AspNetCore;
    using Exceptions;
    using FluentValidation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.Cors.Internal;
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

        public SwaggerOptions Swagger { get; } = new SwaggerOptions();

        public class SwaggerOptions
        {
            public Func<IApiVersionDescriptionProvider, ApiVersionDescription, Info> ApiInfo { get; set; }
            public string[] XmlCommentPaths { get; set; } = null;
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
            public Action<FluentValidationMvcConfiguration> FluentValidation { get; set; }
            public Action<MvcDataAnnotationsLocalizationOptions> DataAnnotationsLocalization { get; set; }
            public Action<AuthorizationOptions> Authorization { get; set; }

            public Action<IMvcCoreBuilder> AfterMvc { get; set; }
            public Action<IHealthChecksBuilder> AfterHealthChecks { get; set; }
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

            services.TryAddEnumerable(ServiceDescriptor.Transient<IApiControllerSpecification, ApiControllerSpec>());

            var configuredCorsMethods = new[]
            {
                HttpMethod.Get.Method,
                HttpMethod.Head.Method,
                HttpMethod.Post.Method,
                HttpMethod.Put.Method,
                HttpMethod.Patch.Method,
                HttpMethod.Delete.Method
            }.Union(options.Cors.Methods ?? new string[0]).Distinct().ToArray();

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
            }.Union(options.Cors.Headers ?? new string[0]).Distinct().ToArray();

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
            }.Union(options.Cors.ExposedHeaders ?? new string[0]).Distinct().ToArray();

            var mvcBuilder = services
                .AddMvcCore(cfg =>
                {
                    cfg.RespectBrowserAcceptHeader = false;
                    cfg.ReturnHttpNotAcceptable = true;

                    cfg.Filters.Add(new LoggingFilterFactory());
                    cfg.Filters.Add(new CorsAuthorizationFilterFactory(StartupHelpers.AllowSpecificOrigin));
                    cfg.Filters.Add<OperationCancelledExceptionFilter>();

                    cfg.Filters.Add(new DataDogTracingFilter());
                })

                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)

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

                .AddJsonFormatters()
                .AddJsonOptions(cfg => cfg.SerializerSettings.ConfigureDefaultForApi())

                .AddXmlDataContractSerializerFormatters()

                .AddApiExplorer();

            options.MiddlewareHooks.AfterMvc?.Invoke(mvcBuilder);

            var healthChecksBuilder = services.AddHealthChecks();
            options.MiddlewareHooks.AfterHealthChecks?.Invoke(healthChecksBuilder);

            mvcBuilder
                .Services

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

                .AddApiVersioning(cfg => cfg.ReportApiVersions = true)

                .AddSwagger<T>(options.Swagger.ApiInfo, options.Swagger.XmlCommentPaths ?? new string[0])

                .AddResponseCompression(cfg =>
                {
                    cfg.EnableForHttps = true;

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
                .Configure<GzipCompressionProviderOptions>(cfg => cfg.Level = CompressionLevel.Fastest);

            ValidatorOptions.DisplayNameResolver =
                (type, member, expression) =>
                    member != null
                        ? GlobalStringLocalizer.Instance.GetLocalizer<TSharedResources>().GetString(() => member.Name)
                        : null;

            return services;
        }
    }
}
