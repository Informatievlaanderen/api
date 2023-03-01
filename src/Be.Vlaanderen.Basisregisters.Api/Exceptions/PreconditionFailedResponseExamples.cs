namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;
    using ProblemDetails = BasicApiProblem.ProblemDetails;

    public class PreconditionFailedResponseExamples : IExamplesProvider<ProblemDetails>
    {
        protected string ApiVersion { get; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ProblemDetailsHelper _problemDetailsHelper;

        public PreconditionFailedResponseExamples(
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
                HttpStatus = StatusCodes.Status412PreconditionFailed,
                Title = ProblemDetails.DefaultTitle,
                Detail = "Als de If-Match header niet overeenkomt met de laatste ETag.",
                ProblemTypeUri = "urn:be.vlaanderen.basisregisters.api:precondition-failed",
                ProblemInstanceUri = _problemDetailsHelper.GetInstanceUri(_httpContextAccessor.HttpContext, ApiVersion)
            };
    }

    public class PreconditionFailedResponseExamplesV2 : PreconditionFailedResponseExamples
    {
        public PreconditionFailedResponseExamplesV2(
            IHttpContextAccessor httpContextAccessor,
            ProblemDetailsHelper problemDetailsHelper) : base(httpContextAccessor, problemDetailsHelper, "v2")
        { }
    }
}
