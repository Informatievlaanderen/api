namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System.Collections.Generic;
    using BasicApiProblem;
    using FluentValidation;
    using FluentValidation.Results;
    using Swashbuckle.AspNetCore.Filters;

    public class ValidationErrorResponseExamples : IExamplesProvider
    {
        public object GetExamples() =>
            new ValidationProblemDetails(new ValidationException(string.Empty, new List<ValidationFailure>
            {
                new ValidationFailure(string.Empty, "Voornaam is verplicht."),
                new ValidationFailure(string.Empty, "Naam is verplicht.")
            }));
    }
}
