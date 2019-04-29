namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using BasicApiProblem;
    using FluentValidation;
    using FluentValidation.Internal;
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Http;

    public class ValidationExceptionHandler : DefaultExceptionHandler<ValidationException>
    {
        protected override ProblemDetails GetApiProblemFor(ValidationException exception, HttpContext context)
        {
            var problemDetails = new ValidationProblemDetails(exception);
            problemDetails.ProblemInstanceUri = context.GetProblemInstanceUri();
            return problemDetails;
        }
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
                cancellationToken,
                (IValidatorSelector)null, ruleSet);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
    }
}
