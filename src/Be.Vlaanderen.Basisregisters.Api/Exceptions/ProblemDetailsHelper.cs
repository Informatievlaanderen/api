namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using Generators.Guid;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using ProblemDetails = BasicApiProblem.ProblemDetails;

    public class ProblemDetailsHelper
    {
        private readonly StartupConfigureOptions.ServerOptions? _configuration;
        private string BaseUrl => _configuration?.BaseUrl ?? string.Empty;
        private string NamespaceOverride => _configuration?.ProblemDetailsTypeNamespaceOverride ?? string.Empty;

        public ProblemDetailsHelper(StartupConfigureOptions? configurationOptions)
            => _configuration = configurationOptions?.Server;

        private static readonly Guid ProblemDetailsSeed = Guid.Parse("21c33bd5-adfa-4f98-b07b-2b83bc00bc99");

        public string GetInstanceBaseUri()
            => $"{BaseUrl}/v1/foutmeldingen";

        public string GetInstanceUri(HttpContext httpContext)
        {
            // this is the same behaviour that Asp.Net core uses
            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            var problemBaseInstanceUri = GetInstanceBaseUri();
            return !string.IsNullOrWhiteSpace(traceId)
                ? $"{problemBaseInstanceUri}/{Deterministic.Create(ProblemDetailsSeed, traceId):N}"
                : $"{problemBaseInstanceUri}/{ProblemDetails.GetProblemNumber()}";
        }

        public string GetExceptionTypeUriFor<T>(T exception) where T : Exception
            => GetExceptionTypeUriFor<T>();

        public string GetExceptionTypeUriFor<T>() where T : Exception
            => string.IsNullOrWhiteSpace(NamespaceOverride)
                ? ProblemDetails.GetTypeUriFor<T>()
                : ProblemDetails.GetTypeUriFor<T>(NamespaceOverride);

        public string RewriteExceptionTypeFrom(ProblemDetails problemDetails)
        {
            var exceptionName = problemDetails
                ?.ProblemTypeUri
                .Split(":", StringSplitOptions.RemoveEmptyEntries)
                .LastOrDefault();

            if (string.IsNullOrWhiteSpace(exceptionName))
                return string.Empty;

            var exceptionType = AssemblyBuilder
                .DefineDynamicAssembly(new AssemblyName("CustomProblemDetailsHelperAssembly"), AssemblyBuilderAccess.Run)
                .DefineDynamicModule("CustomProblemDetailsHelperModule")
                .DefineType(
                    $"{exceptionName}Exception",
                    TypeAttributes.Public | TypeAttributes.Class,
                    typeof(Exception))
                .CreateType();

            dynamic customException = Activator.CreateInstance(exceptionType ?? typeof(Exception)) ?? new Exception();
            return GetExceptionTypeUriFor(customException);
        }
    }

    public static class ProblemDetailsContentHelperExtensions 
    {
        public static void SetTraceId(this ProblemDetails details, ProblemDetailsHelper problemDetailsHelper, HttpContext httpContext)
            => details.ProblemInstanceUri = problemDetailsHelper.GetInstanceUri(httpContext);
    }
}
