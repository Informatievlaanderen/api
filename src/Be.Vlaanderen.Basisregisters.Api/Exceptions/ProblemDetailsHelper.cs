namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using System.Diagnostics;
    using BasicApiProblem;
    using Generators.Guid;
    using Microsoft.AspNetCore.Http;

    public static class ProblemDetailsHelper
    {
        public static readonly Guid ProblemDetails = Guid.Parse("21c33bd5-adfa-4f98-b07b-2b83bc00bc99");

        public static void SetTraceId(ProblemDetails details, HttpContext httpContext)
            => details.ProblemInstanceUri = httpContext.GetProblemInstanceUri();

        public static string GetProblemInstanceUri(this HttpContext httpContext)
        {
            // this is the same behaviour that Asp.Net core uses
            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            return !string.IsNullOrWhiteSpace(traceId)
                ? $"urn:trace:{Deterministic.Create(ProblemDetails, traceId):N}"
                : $"urn:problem:{BasicApiProblem.ProblemDetails.GetProblemNumber()}";
        }
    }
}
