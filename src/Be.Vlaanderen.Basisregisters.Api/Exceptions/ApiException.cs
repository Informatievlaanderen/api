namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using Microsoft.AspNetCore.Http;

    /// <inheritdoc />
    [Serializable]
    public class ApiException : Exception
    {
        /// <summary>
        /// HTTP status code.
        /// </summary>
        public int StatusCode { get; } = StatusCodes.Status500InternalServerError;

        /// <inheritdoc />
        public ApiException() { }

        /// <inheritdoc />
        public ApiException(string message) : base(message)  { }

        /// <inheritdoc />
        public ApiException(string message, Exception inner) : base(message, inner) { }

        /// <inheritdoc />
        public ApiException(string message, int statusCode) : base(message) => StatusCode = statusCode;

        /// <inheritdoc />
        public ApiException(string message, int statusCode, Exception inner) : base(message, inner) => StatusCode = statusCode;

        /// <inheritdoc />
        protected ApiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
