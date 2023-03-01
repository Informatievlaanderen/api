namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using BasicApiProblem;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class UnauthorizedResponseExamples : IExamplesProvider<ProblemDetails>
    {
        protected string ApiVersion { get; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ProblemDetailsHelper _problemDetailsHelper;

        public UnauthorizedResponseExamples(
            IHttpContextAccessor httpContextAccessor,
            ProblemDetailsHelper problemDetailsHelper,
            string apiVersion = "v1")
        {
            _httpContextAccessor = httpContextAccessor;
            _problemDetailsHelper = problemDetailsHelper;
            ApiVersion = apiVersion;
        }

        public ProblemDetails GetExamples() =>
            new ProblemDetails
            {
                ProblemTypeUri = "urn:be.vlaanderen.basisregisters.api:unauthorized",
                HttpStatus = StatusCodes.Status401Unauthorized,
                Title = ProblemDetails.DefaultTitle,
                Detail = "API key verplicht.",
                ProblemInstanceUri = _problemDetailsHelper.GetInstanceUri(_httpContextAccessor.HttpContext, ApiVersion)
            };
    }

    public class UnauthorizedResponseExamplesV2 : UnauthorizedResponseExamples
    {
        public UnauthorizedResponseExamplesV2(
            IHttpContextAccessor httpContextAccessor,
            ProblemDetailsHelper problemDetailsHelper) : base(httpContextAccessor, problemDetailsHelper, "v2")
        { }
    }
}
