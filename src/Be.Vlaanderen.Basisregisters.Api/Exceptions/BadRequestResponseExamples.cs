namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using BasicApiProblem;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class BadRequestResponseExamples : IExamplesProvider
    {
        public object GetExamples() =>
            new ProblemDetails
            {
                HttpStatus = StatusCodes.Status400BadRequest,
                Title = ProblemDetails.DefaultTitle,
                Detail = string.Empty,
                ProblemInstanceUri = ProblemDetails.GetProblemNumber()
            };
    }
}
