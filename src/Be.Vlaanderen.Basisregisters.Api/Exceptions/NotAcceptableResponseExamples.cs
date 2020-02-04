namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;
    using ProblemDetails = BasicApiProblem.ProblemDetails;

    public class NotAcceptableResponseExamples : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples() =>
            new ProblemDetails
            {
                ProblemTypeUri = "urn:be.vlaanderen.basisregisters.api:not-acceptable",
                HttpStatus = StatusCodes.Status406NotAcceptable,
                Title = ProblemDetails.DefaultTitle,
                Detail = "Het gevraagde formaat is niet beschikbaar.",
                ProblemInstanceUri = ProblemDetails.GetProblemNumber()
            };
    }
}
