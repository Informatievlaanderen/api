namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AggregateSource;
    using Microsoft.AspNetCore.Http;
    using ProblemDetails = BasicApiProblem.ProblemDetails;

    public abstract class DefaultExceptionHandler<T> : IExceptionHandler
        where T : Exception
    {
        public Type HandledExceptionType => typeof(T);

        public bool Handles(Exception exception) => null != Cast(exception);

        public Task<ProblemDetails> GetApiProblemFor(HttpContext context, Exception exception)
        {
            var typedException = Cast(exception);
            if (null == typedException)
            {
                throw new InvalidCastException("Could not cast exception to handled type!");
            }

            var problem = GetApiProblemFor(context, typedException);
            return Task.FromResult(problem);
        }

        private static T? Cast(Exception exception) => exception as T;

        protected abstract ProblemDetails GetApiProblemFor(HttpContext context, T exception);
    }

    internal class DefaultExceptionHandlers
    {
        public static IEnumerable<IExceptionHandler> GetHandlers(ProblemDetailsHelper problemDetailsHelper) => new IExceptionHandler[]
        {
            new DomainExceptionHandler(problemDetailsHelper),
            new ApiExceptionHandler(problemDetailsHelper),
            new AggregateNotFoundExceptionHandler(problemDetailsHelper),
            new ValidationExceptionHandler(problemDetailsHelper),
            new HttpRequestExceptionHandler(problemDetailsHelper),
            new DBConcurrencyExceptionHandler(problemDetailsHelper),
            new NotImplementedExceptionHandler(problemDetailsHelper)
        };

        private sealed class DomainExceptionHandler : DefaultExceptionHandler<DomainException>
        {
            private readonly ProblemDetailsHelper _problemDetailsHelper;

            public DomainExceptionHandler(ProblemDetailsHelper problemDetailsHelper)
                => _problemDetailsHelper = problemDetailsHelper;

            protected override ProblemDetails GetApiProblemFor(HttpContext context, DomainException exception) =>
                new ProblemDetails
                {
                    HttpStatus = StatusCodes.Status400BadRequest,
                    Title = ProblemDetails.DefaultTitle,
                    Detail = exception.Message,
                    ProblemTypeUri = _problemDetailsHelper.GetExceptionTypeUriFor(exception),
                    ProblemInstanceUri = $"{_problemDetailsHelper.GetInstanceBaseUri(context)}/{ProblemDetails.GetProblemNumber()}"
                };
        }

        private sealed class ApiExceptionHandler : DefaultExceptionHandler<ApiException>
        {
            private readonly ProblemDetailsHelper _problemDetailsHelper;

            public ApiExceptionHandler(ProblemDetailsHelper problemDetailsHelper)
                => _problemDetailsHelper = problemDetailsHelper;

            protected override ProblemDetails GetApiProblemFor(HttpContext context, ApiException exception) =>
                new ProblemDetails
                {
                    HttpStatus = exception.StatusCode,
                    Title = ProblemDetails.DefaultTitle,
                    Detail = exception.Message,
                    ProblemTypeUri = _problemDetailsHelper.GetExceptionTypeUriFor(exception),
                    ProblemInstanceUri = $"{_problemDetailsHelper.GetInstanceBaseUri(context)}/{ProblemDetails.GetProblemNumber()}"
                };
        }

        private sealed class AggregateNotFoundExceptionHandler : DefaultExceptionHandler<AggregateNotFoundException>
        {
            private readonly ProblemDetailsHelper _problemDetailsHelper;

            public AggregateNotFoundExceptionHandler(ProblemDetailsHelper problemDetailsHelper)
                => _problemDetailsHelper = problemDetailsHelper;

            protected override ProblemDetails GetApiProblemFor(HttpContext context, AggregateNotFoundException exception) =>
                new ProblemDetails
                {
                    HttpStatus = StatusCodes.Status400BadRequest,
                    Title = "Deze actie is niet geldig!",
                    Detail = $"De resource met id '{exception.Identifier}' werd niet gevonden.",
                    ProblemTypeUri = _problemDetailsHelper.GetExceptionTypeUriFor(exception),
                    ProblemInstanceUri = $"{_problemDetailsHelper.GetInstanceBaseUri(context)}/{ProblemDetails.GetProblemNumber()}"
                };
        }

        // This will map HttpRequestException to the 503 Service Unavailable status code.
        private sealed class HttpRequestExceptionHandler : DefaultExceptionHandler<HttpRequestException>
        {
            private readonly ProblemDetailsHelper _problemDetailsHelper;

            public HttpRequestExceptionHandler(ProblemDetailsHelper problemDetailsHelper)
                => _problemDetailsHelper = problemDetailsHelper;

            protected override ProblemDetails GetApiProblemFor(HttpContext context, HttpRequestException exception) =>
                new ProblemDetails
                {
                    HttpStatus = StatusCodes.Status503ServiceUnavailable,
                    Title = ProblemDetails.DefaultTitle,
                    Detail = exception.Message,
                    ProblemTypeUri = _problemDetailsHelper.GetExceptionTypeUriFor(exception),
                    ProblemInstanceUri = $"{_problemDetailsHelper.GetInstanceBaseUri(context)}/{ProblemDetails.GetProblemNumber()}"
                };
        }

        // This will map DBConcurrencyException to the 409 Conflict status code.
        private sealed class DBConcurrencyExceptionHandler : DefaultExceptionHandler<DBConcurrencyException>
        {
            private readonly ProblemDetailsHelper _problemDetailsHelper;

            public DBConcurrencyExceptionHandler(ProblemDetailsHelper problemDetailsHelper)
                => _problemDetailsHelper = problemDetailsHelper;

            protected override ProblemDetails GetApiProblemFor(HttpContext context, DBConcurrencyException exception) =>
                new ProblemDetails
                {
                    HttpStatus = StatusCodes.Status409Conflict,
                    Title = ProblemDetails.DefaultTitle,
                    Detail = exception.Message,
                    ProblemTypeUri = _problemDetailsHelper.GetExceptionTypeUriFor(exception),
                    ProblemInstanceUri = $"{_problemDetailsHelper.GetInstanceBaseUri(context)}/{ProblemDetails.GetProblemNumber()}"
                };
        }

        // This will map NotImplementedException to the 501 Not Implemented status code.
        private sealed class NotImplementedExceptionHandler : DefaultExceptionHandler<NotImplementedException>
        {
            private readonly ProblemDetailsHelper _problemDetailsHelper;

            public NotImplementedExceptionHandler(ProblemDetailsHelper problemDetailsHelper)
                => _problemDetailsHelper = problemDetailsHelper;

            protected override ProblemDetails GetApiProblemFor(HttpContext context, NotImplementedException exception) =>
                new ProblemDetails
                {
                    HttpStatus = StatusCodes.Status501NotImplemented,
                    Title = ProblemDetails.DefaultTitle,
                    Detail = exception.Message,
                    ProblemTypeUri = _problemDetailsHelper.GetExceptionTypeUriFor(exception),
                    ProblemInstanceUri = $"{_problemDetailsHelper.GetInstanceBaseUri(context)}/{ProblemDetails.GetProblemNumber()}"
                };
        }
    }
}
