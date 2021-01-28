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

        public Task<ProblemDetails> GetApiProblemFor(Exception exception)
        {
            var typedException = Cast(exception);
            if (null == typedException)
                throw new InvalidCastException("Could not cast exception to handled type!");

            var problem = GetApiProblemFor(typedException);
            return Task.FromResult(problem);
        }

        private static T Cast(Exception exception) => exception as T;

        protected abstract ProblemDetails GetApiProblemFor(T exception);
    }

    internal class DefaultExceptionHandlers
    {
        public static IEnumerable<IExceptionHandler> GetHandlers(StartupConfigureOptions? options) => new IExceptionHandler[]
        {
            new DomainExceptionHandler(options),
            new ApiExceptionHandler(options),
            new AggregateNotFoundExceptionHandler(options),
            new ValidationExceptionHandler(options),
            new HttpRequestExceptionHandler(options),
            new DBConcurrencyExceptionHandler(options),
            new NotImplementedExceptionHandler(options),
        };

        private class DomainExceptionHandler : DefaultExceptionHandler<DomainException>
        {
            private readonly ProblemDetailsHelper _problemDetailsHelper;

            public DomainExceptionHandler(StartupConfigureOptions? options)
                => _problemDetailsHelper = new ProblemDetailsHelper(options);

            protected override ProblemDetails GetApiProblemFor(DomainException exception) =>
                new ProblemDetails
                {
                    HttpStatus = StatusCodes.Status400BadRequest,
                    Title = ProblemDetails.DefaultTitle,
                    Detail = exception.Message,
                    ProblemTypeUri = _problemDetailsHelper.GetExceptionTypeUriFor(exception),
                    ProblemInstanceUri = $"{_problemDetailsHelper.GetInstanceBaseUri()}/{ProblemDetails.GetProblemNumber()}"
                };
        }

        private class ApiExceptionHandler : DefaultExceptionHandler<ApiException>
        {
            private readonly ProblemDetailsHelper _problemDetailsHelper;

            public ApiExceptionHandler(StartupConfigureOptions? options)
                => _problemDetailsHelper = new ProblemDetailsHelper(options);

            protected override ProblemDetails GetApiProblemFor(ApiException exception) =>
                new ProblemDetails
                {
                    HttpStatus = exception.StatusCode,
                    Title = ProblemDetails.DefaultTitle,
                    Detail = exception.Message,
                    ProblemTypeUri = _problemDetailsHelper.GetExceptionTypeUriFor(exception),
                    ProblemInstanceUri = $"{_problemDetailsHelper.GetInstanceBaseUri()}/{ProblemDetails.GetProblemNumber()}"
                };
        }

        private class AggregateNotFoundExceptionHandler : DefaultExceptionHandler<AggregateNotFoundException>
        {
            private readonly ProblemDetailsHelper _problemDetailsHelper;

            public AggregateNotFoundExceptionHandler(StartupConfigureOptions? options)
                => _problemDetailsHelper = new ProblemDetailsHelper(options);

            protected override ProblemDetails GetApiProblemFor(AggregateNotFoundException exception) =>
                new ProblemDetails
                {
                    HttpStatus = StatusCodes.Status400BadRequest,
                    Title = "Deze actie is niet geldig!",
                    Detail = $"De resource met id '{exception.Identifier}' werd niet gevonden.",
                    ProblemTypeUri = _problemDetailsHelper.GetExceptionTypeUriFor(exception),
                    ProblemInstanceUri = $"{_problemDetailsHelper.GetInstanceBaseUri()}/{ProblemDetails.GetProblemNumber()}"
                };
        }

        // This will map HttpRequestException to the 503 Service Unavailable status code.
        private class HttpRequestExceptionHandler : DefaultExceptionHandler<HttpRequestException>
        {
            private readonly ProblemDetailsHelper _problemDetailsHelper;

            public HttpRequestExceptionHandler(StartupConfigureOptions? options)
                => _problemDetailsHelper = new ProblemDetailsHelper(options);

            protected override ProblemDetails GetApiProblemFor(HttpRequestException exception) =>
                new ProblemDetails
                {
                    HttpStatus = StatusCodes.Status503ServiceUnavailable,
                    Title = ProblemDetails.DefaultTitle,
                    Detail = exception.Message,
                    ProblemTypeUri = _problemDetailsHelper.GetExceptionTypeUriFor(exception),
                    ProblemInstanceUri = $"{_problemDetailsHelper.GetInstanceBaseUri()}/{ProblemDetails.GetProblemNumber()}"
                };
        }

        // This will map DBConcurrencyException to the 409 Conflict status code.
        private class DBConcurrencyExceptionHandler : DefaultExceptionHandler<DBConcurrencyException>
        {
            private readonly ProblemDetailsHelper _problemDetailsHelper;

            public DBConcurrencyExceptionHandler(StartupConfigureOptions? options)
                => _problemDetailsHelper = new ProblemDetailsHelper(options);

            protected override ProblemDetails GetApiProblemFor(DBConcurrencyException exception) =>
                new ProblemDetails
                {
                    HttpStatus = StatusCodes.Status409Conflict,
                    Title = ProblemDetails.DefaultTitle,
                    Detail = exception.Message,
                    ProblemTypeUri = _problemDetailsHelper.GetExceptionTypeUriFor(exception),
                    ProblemInstanceUri = $"{_problemDetailsHelper.GetInstanceBaseUri()}/{ProblemDetails.GetProblemNumber()}"
                };
        }

        // This will map NotImplementedException to the 501 Not Implemented status code.
        private class NotImplementedExceptionHandler : DefaultExceptionHandler<NotImplementedException>
        {
            private readonly ProblemDetailsHelper _problemDetailsHelper;

            public NotImplementedExceptionHandler(StartupConfigureOptions? options)
                => _problemDetailsHelper = new ProblemDetailsHelper(options);

            protected override ProblemDetails GetApiProblemFor(NotImplementedException exception) =>
                new ProblemDetails
                {
                    HttpStatus = StatusCodes.Status501NotImplemented,
                    Title = ProblemDetails.DefaultTitle,
                    Detail = exception.Message,
                    ProblemTypeUri = _problemDetailsHelper.GetExceptionTypeUriFor(exception),
                    ProblemInstanceUri = $"{_problemDetailsHelper.GetInstanceBaseUri()}/{ProblemDetails.GetProblemNumber()}"
                };
        }
    }
}
