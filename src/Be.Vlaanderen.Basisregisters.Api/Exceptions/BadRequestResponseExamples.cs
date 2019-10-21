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
                Detail = "<meer informatie over de foutieve data>",
                ProblemInstanceUri = ProblemDetails.GetProblemNumber()
            };
    }
}
