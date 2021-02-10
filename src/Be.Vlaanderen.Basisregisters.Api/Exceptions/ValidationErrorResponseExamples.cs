namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System.Collections.Generic;
    using FluentValidation;
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;
    using ValidationProblemDetails = BasicApiProblem.ValidationProblemDetails;

    public class ValidationErrorResponseExamples : IExamplesProvider<ValidationProblemDetails>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ProblemDetailsHelper _problemDetailsHelper;

        public ValidationErrorResponseExamples(
            IHttpContextAccessor httpContextAccessor,
            ProblemDetailsHelper problemDetailsHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _problemDetailsHelper = problemDetailsHelper;
        }

        public ValidationProblemDetails GetExamples() =>
            new ValidationProblemDetails(new ValidationException(string.Empty, new List<ValidationFailure>
            {
                new ValidationFailure("Voornaam", "Veld is verplicht."),
                new ValidationFailure("Naam", "Veld mag niet kleiner zijn dan 4 karakters."),
                new ValidationFailure("Naam", "Veld mag niet groter zijn dan 100 karakters.")
            }))
            {
                ProblemInstanceUri = _problemDetailsHelper.GetInstanceUri(_httpContextAccessor.HttpContext)
            };
    }
}
