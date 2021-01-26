namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using ProblemDetails = BasicApiProblem.ProblemDetails;

    public class ApiProblemDetailsException : ApiException
    {
        public ProblemDetails Details { get; }

        public ApiProblemDetailsException(string message, int statusCode, ProblemDetails problemDetails, Exception innerException)
            : base(message, statusCode, innerException)
        {
            Details = problemDetails ?? throw new ArgumentNullException(nameof(problemDetails));
        }
    }
}
