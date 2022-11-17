namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
    using AggregateSource;
    using BasicApiProblem;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using ProblemDetails = BasicApiProblem.ProblemDetails;

    public class ExceptionHandler
    {
        private readonly ILogger<ApiExceptionHandler> _logger;
        private readonly IEnumerable<ApiProblemDetailsExceptionMapping> _apiProblemDetailsExceptionMappers;
        private readonly IEnumerable<IExceptionHandler> _exceptionHandlers;
        private readonly ProblemDetailsHelper _problemDetailsHelper;

        public ExceptionHandler(
            ILogger<ApiExceptionHandler> logger,
            IEnumerable<ApiProblemDetailsExceptionMapping> apiProblemDetailsExceptionMappings,
            IEnumerable<IExceptionHandler> customExceptionHandlers,
            ProblemDetailsHelper problemDetailsHelper)
        {
            _logger = logger;
            _apiProblemDetailsExceptionMappers = apiProblemDetailsExceptionMappings;
            _problemDetailsHelper = problemDetailsHelper ?? throw new ArgumentNullException(nameof(problemDetailsHelper));
            _exceptionHandlers = customExceptionHandlers.Concat(DefaultExceptionHandlers.GetHandlers(_problemDetailsHelper));
        }

        /// <summary>Sets the exception result as HttpResponse</summary>
        public async Task HandleException(Exception exception, HttpContext context)
        {
            if (exception is ApiProblemDetailsException problemDetailsException)
            {
                var problemDetailMappings = _apiProblemDetailsExceptionMappers
                    .Where(mapping => mapping.Handles(problemDetailsException))
                    .ToList();

                if (problemDetailMappings.Count == 1)
                {
                    throw new ProblemDetailsException(problemDetailMappings.First().Map(problemDetailsException, _problemDetailsHelper));
                }

                if (problemDetailMappings.Count > 1)
                {
                    _logger.LogWarning($"Multiple mappings for {nameof(ApiProblemDetailsException)} found. Skipping specific mapping.");
                }
            }

            var exceptionHandler = _exceptionHandlers.FirstOrDefault(handler => handler.Handles(exception));

            if (exceptionHandler == null)
            {
                throw new ProblemDetailsException(HandleUnhandledException(exception));
            }

            var problem = await exceptionHandler.GetApiProblemFor(exception);
            problem.ProblemInstanceUri = _problemDetailsHelper.GetInstanceUri(context);

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
                Detail = "",
                ProblemTypeUri = _problemDetailsHelper.GetExceptionTypeUriFor<UnhandledException>(),
                ProblemInstanceUri = ProblemDetails.GetProblemNumber()
            };

            if (exception != null)
            {
                _logger.LogError(0, exception, "[{ErrorNumber}] Unhandled exception!", problemResponse.ProblemInstanceUri);
            }

            return problemResponse;
        }

        [Serializable]
        public sealed class UnhandledException : Exception
        {
            public UnhandledException()
            { }

            private UnhandledException(SerializationInfo info, StreamingContext context)
                : base(info, context)
            { }
        }
    }
}
