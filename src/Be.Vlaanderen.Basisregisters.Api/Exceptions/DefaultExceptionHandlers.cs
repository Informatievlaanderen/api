namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AggregateSource;
    using BasicApiProblem;
    using Microsoft.AspNetCore.Http;

    public abstract class DefaultExceptionHandler<T> : IExceptionHandler
        where T : Exception
    {
        public Type HandledExceptionType => typeof(T);

        public bool Handles(Exception exception) => null != Cast(exception);

        public Task<ProblemDetails> GetApiProblemFor(Exception exception, HttpContext context)
        {
            var typedException = Cast(exception);
            if (null == typedException)
                throw new InvalidCastException("Could not cast exception to handled type!");

            var problem = GetApiProblemFor(typedException, context);
            return Task.FromResult(problem);
        }

        private static T Cast(Exception exception) => exception as T;

        protected abstract ProblemDetails GetApiProblemFor(T exception, HttpContext context);
    }

    internal class DefaultExceptionHandlers
    {
        public static IEnumerable<IExceptionHandler> Handlers => new IExceptionHandler[]
        {
            new DomainExceptionHandler(),
            new ApiExceptionHandler(),
            new AggregateNotFoundExceptionHandler(),
            new ValidationExceptionHandler(),
            new HttpRequestExceptionHandler(),
            new DBConcurrencyExceptionHandler(),
            new NotImplementedExceptionHandler(), 
        };

        private class DomainExceptionHandler : DefaultExceptionHandler<DomainException>
        {
            protected override ProblemDetails GetApiProblemFor(DomainException exception, HttpContext context) =>
                new ProblemDetails
                {
                    HttpStatus = StatusCodes.Status400BadRequest,
                    Title = ProblemDetails.DefaultTitle,
                    Detail = exception.Message,
                    ProblemInstanceUri = context.GetProblemInstanceUri(),
                    ProblemTypeUri = ProblemDetails.GetTypeUriFor(exception)
                };
        }

        private class ApiExceptionHandler : DefaultExceptionHandler<ApiException>
        {
            protected override ProblemDetails GetApiProblemFor(ApiException exception, HttpContext context) =>
                new ProblemDetails
                {
                    HttpStatus = exception.StatusCode,
                    Title = ProblemDetails.DefaultTitle,
                    Detail = exception.Message,
                    ProblemInstanceUri = context.GetProblemInstanceUri(),
                    ProblemTypeUri = ProblemDetails.GetTypeUriFor(exception)
                };
        }

        private class AggregateNotFoundExceptionHandler : DefaultExceptionHandler<AggregateNotFoundException>
        {
            protected override ProblemDetails GetApiProblemFor(AggregateNotFoundException exception, HttpContext context) =>
                new ProblemDetails
                {
                    HttpStatus = StatusCodes.Status400BadRequest,
                    Title = "Deze actie is niet geldig!",
                    Detail = $"De resource met id '{exception.Identifier}' werd niet gevonden.",
                    ProblemInstanceUri = context.GetProblemInstanceUri(),
                    ProblemTypeUri = ProblemDetails.GetTypeUriFor(exception)
                };
        }

        // This will map HttpRequestException to the 503 Service Unavailable status code.
        private class HttpRequestExceptionHandler : DefaultExceptionHandler<HttpRequestException>
        {
            protected override ProblemDetails GetApiProblemFor(HttpRequestException exception, HttpContext context) =>
                new ProblemDetails
                {
                    HttpStatus = StatusCodes.Status503ServiceUnavailable,
                    Title = ProblemDetails.DefaultTitle,
                    Detail = exception.Message,
                    ProblemInstanceUri = context.GetProblemInstanceUri(),
                    ProblemTypeUri = ProblemDetails.GetTypeUriFor(exception)
                };
        }

        // This will map DBConcurrencyException to the 409 Conflict status code.
        private class DBConcurrencyExceptionHandler : DefaultExceptionHandler<DBConcurrencyException>
        {
            protected override ProblemDetails GetApiProblemFor(DBConcurrencyException exception, HttpContext context) =>
                new ProblemDetails
                {
                    HttpStatus = StatusCodes.Status409Conflict,
                    Title = ProblemDetails.DefaultTitle,
                    Detail = exception.Message,
                    ProblemInstanceUri = context.GetProblemInstanceUri(),
                    ProblemTypeUri = ProblemDetails.GetTypeUriFor(exception)
                };
        }

        // This will map NotImplementedException to the 501 Not Implemented status code.
        private class NotImplementedExceptionHandler : DefaultExceptionHandler<NotImplementedException>
        {
            protected override ProblemDetails GetApiProblemFor(NotImplementedException exception, HttpContext context) =>
                new ProblemDetails
                {
                    HttpStatus = StatusCodes.Status501NotImplemented,
                    Title = ProblemDetails.DefaultTitle,
                    Detail = exception.Message,
                    ProblemInstanceUri = context.GetProblemInstanceUri(),
                    ProblemTypeUri = ProblemDetails.GetTypeUriFor(exception)
                };
        }
    }
}
