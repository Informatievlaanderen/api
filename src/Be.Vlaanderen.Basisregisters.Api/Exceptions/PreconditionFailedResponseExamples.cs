namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;
    using ProblemDetails = BasicApiProblem.ProblemDetails;

    public class PreconditionFailedResponseExamples : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples() =>
            new ProblemDetails
            {
                ProblemTypeUri = "urn:be.vlaanderen.basisregisters.api:precondition-failed",
                HttpStatus = StatusCodes.Status412PreconditionFailed,
                Title = ProblemDetails.DefaultTitle,
                Detail = "De gevraagde minimum positie van de event store is nog niet bereikt.",
                ProblemInstanceUri = ProblemDetails.GetProblemNumber()
            };
    }
}
