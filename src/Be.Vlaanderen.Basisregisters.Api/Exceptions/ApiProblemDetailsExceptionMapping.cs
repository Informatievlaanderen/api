namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using BasicApiProblem;

    public abstract class ApiProblemDetailsExceptionMapping
    {
        public bool Handles(ApiProblemDetailsException? exception)
            => exception?.Details != null && HandlesException(exception);

        public ProblemDetails Map(ApiProblemDetailsException exception, ProblemDetailsHelper problemDetailsHelper)
        {
            if (!Handles(exception))
                throw new InvalidCastException("Could not map problem details!");

            return MapException(exception, problemDetailsHelper);
        }

        public abstract bool HandlesException(ApiProblemDetailsException exception);
        public abstract ProblemDetails MapException(ApiProblemDetailsException exception, ProblemDetailsHelper problemDetailsHelper);
    }
}
