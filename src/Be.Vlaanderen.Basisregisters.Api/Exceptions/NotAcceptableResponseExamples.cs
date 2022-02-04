namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;
    using ProblemDetails = BasicApiProblem.ProblemDetails;

    public class NotAcceptableResponseExamples : IExamplesProvider<ProblemDetails>
    {
        // https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html#sec10.3.5
        // The 406 response MUST NOT contain a message-body
        public ProblemDetails GetExamples() => null;
    }
}
