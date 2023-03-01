namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;
    using ProblemDetails = BasicApiProblem.ProblemDetails;

    public class InternalServerErrorResponseExamples : IExamplesProvider<ProblemDetails>
    {
        protected string ApiVersion { get; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ProblemDetailsHelper _problemDetailsHelper;

        public InternalServerErrorResponseExamples(
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
                ProblemTypeUri = "urn:be.vlaanderen.basisregisters.api:internal-server-error",
                HttpStatus = StatusCodes.Status500InternalServerError,
                Title = ProblemDetails.DefaultTitle,
                Detail = "<meer informatie over de interne fout>",
                ProblemInstanceUri = _problemDetailsHelper.GetInstanceUri(_httpContextAccessor.HttpContext, ApiVersion)
            };
    }

    public class InternalServerErrorResponseExamplesV2 : InternalServerErrorResponseExamples
    {
        public InternalServerErrorResponseExamplesV2(
            IHttpContextAccessor httpContextAccessor,
            ProblemDetailsHelper problemDetailsHelper) : base(httpContextAccessor, problemDetailsHelper, "v2")
        { }
    }
}
