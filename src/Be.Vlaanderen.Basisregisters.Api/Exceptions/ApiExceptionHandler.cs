namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System.Collections.Generic;
    using System.Net;
    using System.Reflection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.Extensions.Logging;

    public class ApiExceptionHandler { }

    public static class ApiExceptionHandlerExtension
    {
        private static ILogger<ApiExceptionHandler> _logger;

        public static IApplicationBuilder UseApiExceptionHandler(
            this IApplicationBuilder app,
            ILoggerFactory loggerFactory,
            string corsPolicyName)
            => UseApiExceptionHandling(app, loggerFactory, corsPolicyName, new IExceptionHandler[]{ });

        public static IApplicationBuilder UseApiExceptionHandler(
            this IApplicationBuilder app,
            ILoggerFactory loggerFactory,
            string corsPolicyName,
            IEnumerable<IExceptionHandler> customExceptionHandlers)
            => UseApiExceptionHandling(app, loggerFactory, corsPolicyName, customExceptionHandlers);

        private static IApplicationBuilder UseApiExceptionHandling(
            IApplicationBuilder app,
            ILoggerFactory loggerFactory,
            string corsPolicyName,
            IEnumerable<IExceptionHandler> customExceptionHandlers)
        {
            _logger = loggerFactory.CreateLogger<ApiExceptionHandler>();
            var customHandlers = customExceptionHandlers ?? new IExceptionHandler[]{ };
            var exceptionHandler = new ExceptionHandler(_logger, customHandlers);

            app.UseExceptionHandler(builder =>
            {
                builder.UseCors(corsPolicyName);
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = error?.Error;

                    // Errors happening in the Apply() stuff result in an InvocationException due to the dynamic stuff.
                    if (exception is TargetInvocationException && exception.InnerException != null)
                        exception = exception.InnerException;

                    await exceptionHandler.HandleException(exception, context);
                });
            });

            return app;
        }
    }
}
