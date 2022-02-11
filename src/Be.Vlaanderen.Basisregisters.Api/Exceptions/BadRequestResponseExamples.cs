namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System.Collections.Generic;
    using BasicApiProblem;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class BadRequestResponseExamples : IExamplesProvider<ValidationProblemDetails>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ProblemDetailsHelper _problemDetailsHelper;

        public BadRequestResponseExamples(
            IHttpContextAccessor httpContextAccessor,
            ProblemDetailsHelper problemDetailsHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _problemDetailsHelper = problemDetailsHelper;
        }

        public ValidationProblemDetails GetExamples() =>
            new ValidationProblemDetails
            {
                ValidationErrors = new Dictionary<string, ValidationProblemDetails.Errors>
                {
                    ["Voornaam"] = new ValidationProblemDetails.Errors { new ValidationError("Veld is verplicht.") },
                    ["Naam"] = new ValidationProblemDetails.Errors { new ValidationError("Veld mag niet kleiner zijn dan 4 karakters."), new ValidationError("Veld mag niet groter zijn dan 100 karakters.") }
                },
                ProblemInstanceUri = _problemDetailsHelper.GetInstanceUri(_httpContextAccessor.HttpContext),
                ProblemTypeUri = "urn:be.vlaanderen.basisregisters.api:validation"
            };
    }
}
