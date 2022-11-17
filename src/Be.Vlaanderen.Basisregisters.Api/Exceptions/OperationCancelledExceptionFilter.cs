namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    /// <inheritdoc />
    /// <summary>
    /// Exception filter to deal with cancellationToken exceptions.
    /// </summary>
    public class OperationCancelledExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;
        private const int Status499ClientClosedRequest = 499;

        public OperationCancelledExceptionFilterAttribute(ILoggerFactory loggerFactory)
            => _logger = loggerFactory.CreateLogger<OperationCancelledExceptionFilterAttribute>();

        public override void OnException(ExceptionContext context)
        {
            if (!(context.Exception is OperationCanceledException))
            {
                return;
            }

            _logger.LogInformation("Request was cancelled");
            context.ExceptionHandled = true;
            context.Result = new StatusCodeResult(Status499ClientClosedRequest);
        }
    }
}
