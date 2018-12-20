namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using Microsoft.AspNetCore.Http;

    public class ApiException : Exception
    {
        public int StatusCode { get; }

        public ApiException() => StatusCode = StatusCodes.Status500InternalServerError;

        public ApiException(string message) : base(message) => StatusCode = StatusCodes.Status500InternalServerError;

        public ApiException(string message, int statusCode) : base(message) => StatusCode = statusCode;

        public ApiException(string message, Exception inner) : base(message, inner) { }

        public ApiException(string message, int statusCode, Exception inner) : base(message, inner) => StatusCode = statusCode;
    }
}
