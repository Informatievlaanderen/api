namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using FluentValidation.Internal;
    using FluentValidation.Results;

    public class ValidationExceptionHandling : DefaultExceptionHandler<ValidationException>
    {
        protected override BasicApiProblem GetApiProblemFor(ValidationException exception)
            => new BasicApiValidationProblem(exception);
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

            ValidationResult validationResult = await validator.ValidateAsync(
                instance,
                cancellationToken,
                (IValidatorSelector)null, ruleSet);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors as IEnumerable<ValidationFailure>);
        }
    }
}
