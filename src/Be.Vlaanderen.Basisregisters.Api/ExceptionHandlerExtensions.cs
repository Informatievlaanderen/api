namespace Be.Vlaanderen.Basisregisters.Api
{
    using System;
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// Extensions for exception handler
    /// </summary>
    public static class ExceptionHandlerExtensions
    {
        /// <summary>
        /// Use exception handler allowing status code 404 responses
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IApplicationBuilder UseExceptionHandler404Allowed(this IApplicationBuilder app, Action<IApplicationBuilder> configure)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var subAppBuilder = app.New();
            configure(subAppBuilder);
            var exceptionHandlerPipeline = subAppBuilder.Build();

            return app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = exceptionHandlerPipeline,
                AllowStatusCode404Response = true
            });
        }
    }
}
