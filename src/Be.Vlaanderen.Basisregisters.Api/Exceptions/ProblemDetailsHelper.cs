namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using System.Diagnostics;
    using Generators.Guid;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using ProblemDetails = BasicApiProblem.ProblemDetails;

    public class ProblemDetailsHelper
    {
        private readonly StartupConfigureOptions _configuration;

        public ProblemDetailsHelper(StartupConfigureOptions? configurationOptions)
            => _configuration = configurationOptions;

        private static readonly Guid ProblemDetailsSeed = Guid.Parse("21c33bd5-adfa-4f98-b07b-2b83bc00bc99");

        public string GetInstanceBaseUri()
        {
            var baseHost = _configuration.Server.BaseUrl ?? string.Empty;
            return $"{baseHost}/v1/foutmeldingen";
        }

        public string GetInstanceUri(HttpContext httpContext)
        {
            // this is the same behaviour that Asp.Net core uses
            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            var problemBaseInstanceUri = GetInstanceBaseUri();
            return !string.IsNullOrWhiteSpace(traceId)
                ? $"{problemBaseInstanceUri}/{Deterministic.Create(ProblemDetailsSeed, traceId):N}"
                : $"{problemBaseInstanceUri}/{ProblemDetails.GetProblemNumber()}";
        }

        public string GetExceptionTypeUriFor<T>(T exception)
            where T : Exception
            => GetExceptionTypeUriFor<T>();

        public string GetExceptionTypeUriFor<T>()
            where T : Exception
        {
            var namespaceOverride = _configuration?.Server?.ProblemDetailsTypeNamespaceOverride;
            return string.IsNullOrWhiteSpace(namespaceOverride)
                ? ProblemDetails.GetTypeUriFor<T>()
                : ProblemDetails.GetTypeUriFor<T>(namespaceOverride);
        }

    }

    public static class ProblemDetailsContentHelperExtensions {
        public static string GetProblemInstanceUri(this HttpContext httpContext)
        {
            var configurationOptions = httpContext?.RequestServices?.GetService<StartupConfigureOptions>();
            return new ProblemDetailsHelper(configurationOptions).GetInstanceUri(httpContext);
        }

        public static void SetTraceId(this ProblemDetails details, HttpContext httpContext)
            => details.ProblemInstanceUri = httpContext.GetProblemInstanceUri();
    }
}
