namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class PreconditionFailedResponseExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            return new BasicApiProblem
            {
                HttpStatus = StatusCodes.Status412PreconditionFailed,
                Title = BasicApiProblem.DefaultTitle,
                Detail = "De gevraagde minimum positie van de event store is nog niet bereikt.",
                ProblemInstanceUri = BasicApiProblem.GetProblemNumber()
            };
        }
    }
}
