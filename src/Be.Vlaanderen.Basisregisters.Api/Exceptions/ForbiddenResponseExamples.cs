namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using BasicApiProblem;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class ForbiddenResponseExamples : IExamplesProvider<ProblemDetails>
    {
        protected string ApiVersion { get; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ProblemDetailsHelper _problemDetailsHelper;

        public ForbiddenResponseExamples(
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
                ProblemTypeUri = "urn:be.vlaanderen.basisregisters.api:forbidden",
                HttpStatus = StatusCodes.Status403Forbidden,
                Title = ProblemDetails.DefaultTitle,
                Detail = "U heeft niet de correcte rechten.",
                ProblemInstanceUri = _problemDetailsHelper.GetInstanceUri(_httpContextAccessor.HttpContext, ApiVersion)
            };
    }

    public class ForbiddenResponseExamplesV2 : ForbiddenResponseExamples
    {
        public ForbiddenResponseExamplesV2(
            IHttpContextAccessor httpContextAccessor,
            ProblemDetailsHelper problemDetailsHelper) : base(httpContextAccessor, problemDetailsHelper, "v2")
        { }
    }
}
