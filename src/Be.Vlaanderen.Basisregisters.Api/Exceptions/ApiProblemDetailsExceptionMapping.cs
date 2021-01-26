namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using BasicApiProblem;

    public class ApiProblemDetailsExceptionMapping
    {
        private readonly Func<ApiProblemDetailsException, bool> _handles;
        private readonly Func<ApiProblemDetailsException, ProblemDetails> _mapProblemDetails;

        public ApiProblemDetailsExceptionMapping(
            Func<ApiProblemDetailsException, bool>? handles,
            Func<ApiProblemDetailsException, ProblemDetails>? mapApiProblemDetailException)
        {
            _handles = handles ?? (_ => false) ;
            _mapProblemDetails = mapApiProblemDetailException ?? throw new ArgumentNullException(nameof(mapApiProblemDetailException));
        }

        public bool Handles(ApiProblemDetailsException? exception)
            => exception?.Details != null && _handles(exception);

        public ProblemDetails Map(ApiProblemDetailsException exception)
        {
            if (!Handles(exception))
                throw new InvalidCastException("Could not map problem details!");

            return _mapProblemDetails(exception);
        }
    }
}
