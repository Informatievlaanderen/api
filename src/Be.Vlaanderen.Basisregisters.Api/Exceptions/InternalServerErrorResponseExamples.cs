namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using BasicApiProblem;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class InternalServerErrorResponseExamples : IExamplesProvider
    {
        public object GetExamples() =>
            new ProblemDetails
            {
                HttpStatus = StatusCodes.Status500InternalServerError,
                Title = ProblemDetails.DefaultTitle,
                Detail = "<meer informatie over de interne fout>",
                ProblemInstanceUri = ProblemDetails.GetProblemNumber()
            };
    }
}
