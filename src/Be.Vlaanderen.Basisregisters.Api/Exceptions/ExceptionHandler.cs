namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AggregateSource;
    using BasicApiProblem;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class ExceptionHandler
    {
        private readonly ILogger<ApiExceptionHandler> _logger;
        private readonly IEnumerable<IExceptionHandler> _exceptionHandlers;

        public ExceptionHandler(
            ILogger<ApiExceptionHandler> logger,
            IEnumerable<IExceptionHandler> customExceptionHandlers)
        {
            _logger = logger;
            _exceptionHandlers = customExceptionHandlers.Concat(DefaultExceptionHandlers.Handlers);
        }

        /// <summary>Sets the exception result as HttpResponse</summary>
        public async Task HandleException(Exception exception, HttpContext context)
        {
            var exceptionHandler = _exceptionHandlers.FirstOrDefault(handler => handler.Handles(exception));

            if (exceptionHandler == null)
                throw new ProblemDetailsException(HandleUnhandledException(exception));

            var problem = await exceptionHandler.GetApiProblemFor(exception);
            problem.ProblemInstanceUri = context.GetProblemInstanceUri();

            LogExceptionHandled(exception, problem, exceptionHandler.HandledExceptionType);
            throw new ProblemDetailsException(problem);
        }

        private void LogExceptionHandled(Exception exception, ProblemDetails problemResponse, Type handledExceptionType)
        {
            var exceptionTypeName = typeof(AggregateNotFoundException) == handledExceptionType
                ? "NotFoundException"
                : handledExceptionType.Name;

            _logger.LogInformation(
                0,
                exception,
                "[{ErrorNumber}] {HandledExceptionType} handled: {ExceptionMessage}",
                problemResponse.ProblemInstanceUri, exceptionTypeName, problemResponse.Detail);
        }

        private ProblemDetails HandleUnhandledException(Exception exception)
        {
            var problemResponse = new ProblemDetails
            {
                HttpStatus = StatusCodes.Status500InternalServerError,
                Title = ProblemDetails.DefaultTitle,
                Detail = string.Empty,
                ProblemTypeUri = ProblemDetails.GetTypeUriFor(exception as UnhandledException),
                ProblemInstanceUri = ProblemDetails.GetProblemNumber()
            };

            if (exception != null)
                _logger.LogError(0, exception, "[{ErrorNumber}] Unhandled exception!", problemResponse.ProblemInstanceUri);

            return problemResponse;
        }

        private class UnhandledException : Exception { }
    }
}
