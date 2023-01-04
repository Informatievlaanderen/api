namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using BasicApiProblem;
    using Microsoft.AspNetCore.Http;

    public abstract class ApiProblemDetailsExceptionMapping
    {
        public bool Handles(ApiProblemDetailsException? exception)
            => exception?.Details != null && HandlesException(exception);

        public ProblemDetails Map(HttpContext context, ApiProblemDetailsException exception, ProblemDetailsHelper problemDetailsHelper)
        {
            if (!Handles(exception))
                throw new InvalidCastException("Could not map problem details!");

            return MapException(context, exception, problemDetailsHelper);
        }

        public abstract bool HandlesException(ApiProblemDetailsException exception);
        public abstract ProblemDetails MapException(HttpContext context, ApiProblemDetailsException exception, ProblemDetailsHelper problemDetailsHelper);
    }
}
