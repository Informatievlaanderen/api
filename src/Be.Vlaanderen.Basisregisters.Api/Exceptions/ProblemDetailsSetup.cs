
namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using BasicApiProblem;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using ProblemDetails = BasicApiProblem.ProblemDetails;

    public class ProblemDetailsSetup : IConfigureOptions<ProblemDetailsOptions>
    {
        private IHostingEnvironment Environment { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private ApiBehaviorOptions ApiOptions { get; }

        public ProblemDetailsSetup(
            IHostingEnvironment environment,
            IHttpContextAccessor httpContextAccessor,
            IOptions<ApiBehaviorOptions> apiOptions)
        {
            Environment = environment;
            HttpContextAccessor = httpContextAccessor;
            ApiOptions = apiOptions.Value;
        }

        public void Configure(ProblemDetailsOptions options)
        {
            options.IncludeExceptionDetails = ctx => Environment.IsDevelopment();

            options.MapStatusCode = MapStatusCode;

            // keep consistent with asp.net core 2.2 conventions that adds a tracing value
            options.OnBeforeWriteDetails = (ctx, details) => ProblemDetailsHelper.SetTraceId(details, HttpContextAccessor.HttpContext);
        }

        private ProblemDetails MapStatusCode(HttpContext context, int statusCode)
            => !ApiOptions.SuppressMapClientErrors && ApiOptions.ClientErrorMapping.TryGetValue(statusCode, out var errorData)
                ? new ProblemDetails
                {
                    HttpStatus = statusCode,
                    Title = errorData.Title,
                    ProblemTypeUri = errorData.Link
                }
                : new StatusCodeProblemDetails(statusCode);
    }
}
