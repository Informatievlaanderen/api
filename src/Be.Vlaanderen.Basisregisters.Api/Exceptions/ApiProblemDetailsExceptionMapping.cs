namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using BasicApiProblem;

    public abstract class ApiProblemDetailsExceptionMapping
    {
        public abstract bool Handles(ApiProblemDetailsException exception);
        public abstract ProblemDetails Map(StartupConfigureOptions options, ApiProblemDetailsException exception);
    }
}
