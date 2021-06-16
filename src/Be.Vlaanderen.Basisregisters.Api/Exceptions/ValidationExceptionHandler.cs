namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using FluentValidation.Internal;
    using FluentValidation.Results;
    using ProblemDetails = BasicApiProblem.ProblemDetails;
    using ValidationProblemDetails = BasicApiProblem.ValidationProblemDetails;

    public class ValidationExceptionHandler : DefaultExceptionHandler<ValidationException>
    {
        private readonly ProblemDetailsHelper _problemDetailsHelper;

        public ValidationExceptionHandler(ProblemDetailsHelper problemDetailsHelper)
            => _problemDetailsHelper = problemDetailsHelper;

        protected override ProblemDetails GetApiProblemFor(ValidationException exception)
            => new ValidationProblemDetails(exception)
            {
                ProblemTypeUri = _problemDetailsHelper.GetExceptionTypeUriFor<ValidationException>(),
                ProblemInstanceUri = $"{_problemDetailsHelper.GetInstanceBaseUri()}/{ProblemDetails.GetProblemNumber()}"
            };
    }

    public static class ValidationHelpers
    {
        public static async Task ValidateAndThrowAsync<T>(
            this IValidator<T> validator,
            T instance,
            string ruleSet = null,
            CancellationToken cancellationToken = default)
        {
            if (instance == null)
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("request", "Request cannot be empty.")
                });

            var validationResult = await validator.ValidateAsync(
                instance,
                v =>
                {
                    if(!string.IsNullOrWhiteSpace(ruleSet))
                        v.IncludeRuleSets(ruleSet);
                },
                cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
    }
}
