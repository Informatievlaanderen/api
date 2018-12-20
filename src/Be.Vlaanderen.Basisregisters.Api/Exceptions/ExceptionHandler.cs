namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using AggregateSource;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

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
            if (null != exceptionHandler)
            {
                var problem = await exceptionHandler.GetApiProblemFor(exception);
                LogExceptionHandled(exception, problem, exceptionHandler.HandledExceptionType);
                await SetResponse(context, problem);
                return;
            }

            await HandleUnhandledException(exception, context);
        }

        private void LogExceptionHandled(Exception exception, BasicApiProblem problemResponse, Type handledExceptionType)
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

        private static async Task SetResponse<TBasicApiProblem>(HttpContext context, TBasicApiProblem responseData)
            where TBasicApiProblem : BasicApiProblem
        {
            const JsonSerializerSettings defaultJsonSerializerSettings = null;

            context.Response.StatusCode = responseData.HttpStatus;
            await context
                .Response
                .WriteAsync(JsonConvert.SerializeObject(responseData, responseData.GetType(), defaultJsonSerializerSettings))
                .ConfigureAwait(false);
        }

        private async Task HandleUnhandledException(Exception exception, HttpContext context)
        {
            var problemResponse = new BasicApiProblem
            {
                HttpStatus = (int)HttpStatusCode.InternalServerError,
                Title = BasicApiProblem.DefaultTitle,
                Detail = string.Empty,
                ProblemInstanceUri = BasicApiProblem.GetProblemNumber(),
                ProblemTypeUri = BasicApiProblem.GetTypeUriFor(exception as UnhandledException)
            };

            if (exception != null)
                _logger.LogError(0, exception, "[{ErrorNumber}] Unhandled exception!", problemResponse.ProblemInstanceUri);

            await SetResponse(context, problemResponse);
        }

        private class UnhandledException : Exception { }
    }
}
