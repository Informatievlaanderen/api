namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using BasicApiProblem;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Newtonsoft.Json;

    // https://github.com/Microsoft/aspnet-api-versioning/wiki/Error-Responses

    /// <summary>
    /// Represents the ProblemDetails implementation for creating HTTP error responses related to API versioning.
    /// </summary>
    public class ProblemDetailsResponseProvider : IErrorResponseProvider
    {
        /// <summary>
        /// Creates and returns a new error response given the provided context.
        /// </summary>
        /// <param name="context">The <see cref="ErrorResponseContext">error context</see> used to generate the response.</param>
        /// <returns>The generated <see cref="IActionResult">response</see>.</returns>
        public virtual IActionResult CreateResponse(ErrorResponseContext context)
            => new ObjectResult(CreateErrorContent(context)) { StatusCode = context.StatusCode };

        /// <summary>
        /// Creates the default error content using the given context.
        /// </summary>
        /// <param name="context">The <see cref="ErrorResponseContext">error context</see> used to create the error content.</param>
        /// <returns>An <see cref="object"/> representing the error content.</returns>
        protected virtual object CreateErrorContent(ErrorResponseContext context)
            => new ApiVersionProblemDetails(context);
    }

    public class ApiVersionProblemDetails : StatusCodeProblemDetails
    {
        private readonly Dictionary<string, string> _errorCodeTitles = new Dictionary<string, string>
        {
            {"ApiVersionUnspecified", "API version unspecified."},
            {"UnsupportedApiVersion", "Unsupported API version."},
            {"InvalidApiVersion", "Invalid API version."},
            {"AmbiguousApiVersion", "An API version was specified multiple times with different values."},
        }.ToDictionary(x => x.Key.ToLowerInvariant(), x => x.Value);

        private const string DefaultErrorCodeTitle = "There was a problem determining the API version.";

        /// <summary>Validatie fouten.</summary>
        [JsonProperty("apiVersionError", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DataMember(Name = "apiVersionError", Order = 600, EmitDefaultValue = false)]
        public object InnerError { get; set; }

        public ApiVersionProblemDetails(ErrorResponseContext context) : base(context.StatusCode)
        {
            var errorCode = context.ErrorCode.ToLowerInvariant();

            ProblemTypeUri = $"urn:apiversion:{errorCode}";
            Detail = NullIfEmpty(context.Message);
            InnerError = NewInnerError(context, c => new { Message = c.MessageDetail });

            Title = _errorCodeTitles.ContainsKey(errorCode)
                ? _errorCodeTitles[errorCode]
                : DefaultErrorCodeTitle;

            ProblemInstanceUri = context.Request.HttpContext.GetProblemInstanceUri();
        }

        private static string NullIfEmpty(string value) => string.IsNullOrEmpty(value) ? null : value;

        private static TError NewInnerError<TError>(ErrorResponseContext context, Func<ErrorResponseContext, TError> create)
        {
            if (string.IsNullOrEmpty(context.MessageDetail))
                return default;

            var environment = (IHostingEnvironment)context.Request.HttpContext.RequestServices.GetService(typeof(IHostingEnvironment));

            return environment?.IsDevelopment() == true
                ? create(context)
                : default;
        }
    }
}
