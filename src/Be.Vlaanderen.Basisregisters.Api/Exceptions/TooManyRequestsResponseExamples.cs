namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using BasicApiProblem;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class TooManyRequestsResponseExamples : IExamplesProvider<ProblemDetails>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ProblemDetailsHelper _problemDetailsHelper;

        public TooManyRequestsResponseExamples(
            IHttpContextAccessor httpContextAccessor,
            ProblemDetailsHelper problemDetailsHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _problemDetailsHelper = problemDetailsHelper;
        }

        public ProblemDetails GetExamples() =>
            new ProblemDetails
            {                
                HttpStatus = StatusCodes.Status429TooManyRequests,
                Title = ProblemDetails.DefaultTitle,
                Detail = "U voert teveel requests uit in een korte tijdspanne. Probeer later opnieuw.",
                ProblemTypeUri = "urn:be.vlaanderen.basisregisters.api:throttled",
                ProblemInstanceUri = _problemDetailsHelper.GetInstanceUri(_httpContextAccessor.HttpContext)
            };
    }
}
