namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using BasicApiProblem;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class PreconditionFailedResponseExamples : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples() =>
            new ProblemDetails
            {
                HttpStatus = StatusCodes.Status412PreconditionFailed,
                Title = ProblemDetails.DefaultTitle,
                Detail = "De gevraagde minimum positie van de event store is nog niet bereikt.",
                ProblemInstanceUri = ProblemDetails.GetProblemNumber()
            };
    }
}
