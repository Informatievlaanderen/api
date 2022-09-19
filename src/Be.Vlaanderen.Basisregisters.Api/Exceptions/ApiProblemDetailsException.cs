namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using ProblemDetails = BasicApiProblem.ProblemDetails;

    /// <inheritdoc />
    [Serializable]
    public class ApiProblemDetailsException : ApiException
    {
        public ProblemDetails Details { get; }

        /// <inheritdoc />
        public ApiProblemDetailsException(string message, int statusCode, ProblemDetails problemDetails, Exception innerException)
            : base(message, statusCode, innerException)
        {
            Details = problemDetails ?? throw new ArgumentNullException(nameof(problemDetails));
        }

        /// <inheritdoc />
        protected ApiProblemDetailsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
