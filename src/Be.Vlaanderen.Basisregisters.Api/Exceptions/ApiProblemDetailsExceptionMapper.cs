namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using BasicApiProblem;

    internal class ApiProblemDetailsExceptionMapper
    {
        private readonly StartupConfigureOptions? _options;
        private readonly ApiProblemDetailsExceptionMapping _mapping;

        public ApiProblemDetailsExceptionMapper(
            StartupConfigureOptions? options,
            ApiProblemDetailsExceptionMapping apiProblemDetailsExceptionMapping)
        {
            _options = options;
            _mapping = apiProblemDetailsExceptionMapping ?? throw new ArgumentNullException(nameof(apiProblemDetailsExceptionMapping));
        }

        public bool Handles(ApiProblemDetailsException? exception)
            => exception?.Details != null && _mapping.Handles(exception);

        public ProblemDetails Map(ApiProblemDetailsException exception)
        {
            if (!Handles(exception))
                throw new InvalidCastException("Could not map problem details!");

            return _mapping.Map(_options, exception);
        }
    }
}
