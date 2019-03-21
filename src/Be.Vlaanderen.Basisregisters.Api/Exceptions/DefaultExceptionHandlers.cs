namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using AggregateSource;

    public abstract class DefaultExceptionHandler<T> : IExceptionHandler
        where T : Exception
    {
        public Type HandledExceptionType => typeof(T);

        public bool Handles(Exception exception) => null != Cast(exception);

        public Task<BasicApiProblem> GetApiProblemFor(Exception exception)
        {
            var typedException = Cast(exception);
            if (null == typedException)
                throw new InvalidCastException("Could not cast exception to handled type!");

            var problem = GetApiProblemFor(typedException);
            return Task.FromResult(problem);
        }

        private static T Cast(Exception exception) => exception as T;

        protected abstract BasicApiProblem GetApiProblemFor(T exception);
    }

    internal class DefaultExceptionHandlers
    {
        public static IEnumerable<IExceptionHandler> Handlers => new IExceptionHandler[]
        {
            new DomainExceptionHandler(),
            new ApiExceptionHandler(),
            new AggregateNotFoundExceptionHandling()
        };

        private class DomainExceptionHandler : DefaultExceptionHandler<DomainException>
        {
            protected override BasicApiProblem GetApiProblemFor(DomainException exception) =>
                new BasicApiProblem
                {
                    HttpStatus = (int)HttpStatusCode.BadRequest,
                    Title = BasicApiProblem.DefaultTitle,
                    Detail = exception.Message,
                    ProblemInstanceUri = BasicApiProblem.GetProblemNumber(),
                    ProblemTypeUri = BasicApiProblem.GetTypeUriFor(exception)
                };
        }

        private class ApiExceptionHandler : DefaultExceptionHandler<ApiException>
        {
            protected override BasicApiProblem GetApiProblemFor(ApiException exception) =>
                new BasicApiProblem
                {
                    HttpStatus = exception.StatusCode,
                    Title = BasicApiProblem.DefaultTitle,
                    Detail = exception.Message,
                    ProblemInstanceUri = BasicApiProblem.GetProblemNumber(),
                    ProblemTypeUri = BasicApiProblem.GetTypeUriFor(exception)
                };
        }

        private class AggregateNotFoundExceptionHandling : DefaultExceptionHandler<AggregateNotFoundException>
        {
            protected override BasicApiProblem GetApiProblemFor(AggregateNotFoundException exception) =>
                new BasicApiProblem
                {
                    HttpStatus = (int)HttpStatusCode.BadRequest,
                    Title = "Deze actie is niet geldig!",
                    Detail = $"De resource met id '{exception.Identifier}' werd niet gevonden.",
                    ProblemInstanceUri = BasicApiProblem.GetProblemNumber(),
                    ProblemTypeUri = BasicApiProblem.GetTypeUriFor(exception)
                };
        }
    }
}
