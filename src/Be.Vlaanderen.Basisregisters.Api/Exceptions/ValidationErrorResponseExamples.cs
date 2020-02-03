namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System.Collections.Generic;
    using FluentValidation;
    using FluentValidation.Results;
    using Swashbuckle.AspNetCore.Filters;
    using ValidationProblemDetails = BasicApiProblem.ValidationProblemDetails;

    public class ValidationErrorResponseExamples : IExamplesProvider<ValidationProblemDetails>
    {
        public ValidationProblemDetails GetExamples() =>
            new ValidationProblemDetails(new ValidationException(string.Empty, new List<ValidationFailure>
            {
                new ValidationFailure(string.Empty, "Voornaam is verplicht."),
                new ValidationFailure(string.Empty, "Naam is verplicht.")
            }));
    }
}
