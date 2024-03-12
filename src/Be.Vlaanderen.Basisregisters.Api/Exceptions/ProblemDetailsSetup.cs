namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using BasicApiProblem;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using ProblemDetails = BasicApiProblem.ProblemDetails;
    using ProblemDetailsOptions = BasicApiProblem.ProblemDetailsOptions;

    public class ProblemDetailsSetup : IConfigureOptions<ProblemDetailsOptions>
    {
        private ProblemDetailsHelper ProblemDetailsHelper { get; }
        private IHostingEnvironment Environment { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private ApiBehaviorOptions ApiOptions { get; }

        public ProblemDetailsSetup(
            IHostingEnvironment environment,
            IHttpContextAccessor httpContextAccessor,
            IOptions<ApiBehaviorOptions> apiOptions,
            ProblemDetailsHelper problemDetailsHelper)
        {
            ProblemDetailsHelper = problemDetailsHelper;
            Environment = environment;
            HttpContextAccessor = httpContextAccessor;
            ApiOptions = apiOptions.Value;
        }

        public void Configure(ProblemDetailsOptions options)
        {
            options.MapStatusCode = MapStatusCode;

            // keep consistent with asp.net core 2.2 conventions that adds a tracing value
            options.OnBeforeWriteDetails = (ctx, details) => details.SetTraceId(ProblemDetailsHelper, HttpContextAccessor.HttpContext);
        }

        private ProblemDetails MapStatusCode(HttpContext context, int statusCode)
            => !ApiOptions.SuppressMapClientErrors && ApiOptions.ClientErrorMapping.TryGetValue(statusCode, out var errorData)
                ? new ProblemDetails
                {
                    HttpStatus = statusCode,
                    ProblemTypeUri = errorData.Link,
                    Title = errorData.Title,
                    Detail = string.Empty,
                    ProblemInstanceUri = ProblemDetailsHelper.GetInstanceUri(context)
                }
                : new StatusCodeProblemDetails(statusCode)
                {
                    Detail = string.Empty,
                    ProblemInstanceUri = ProblemDetailsHelper.GetInstanceUri(context)
                };
    }
}
