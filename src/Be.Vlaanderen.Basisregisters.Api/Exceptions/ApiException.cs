namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using Microsoft.AspNetCore.Http;

    public class ApiException : Exception
    {
        public int StatusCode { get; } = StatusCodes.Status500InternalServerError;

        public ApiException() { }

        public ApiException(string message) : base(message)  { }

        public ApiException(string message, Exception inner) : base(message, inner) { }

        public ApiException(string message, int statusCode) : base(message) => StatusCode = statusCode;

        public ApiException(string message, int statusCode, Exception inner) : base(message, inner) => StatusCode = statusCode;
    }
}
