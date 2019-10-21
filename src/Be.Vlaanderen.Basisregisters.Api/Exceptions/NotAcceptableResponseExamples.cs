namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using BasicApiProblem;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class NotAcceptableResponseExamples : IExamplesProvider
    {
        public object GetExamples() =>
            new ProblemDetails
            {
                HttpStatus = StatusCodes.Status406NotAcceptable,
                Title = ProblemDetails.DefaultTitle,
                Detail = "Het gevraagde formaat is niet beschikbaar.",
                ProblemInstanceUri = ProblemDetails.GetProblemNumber()
            };
    }
}
