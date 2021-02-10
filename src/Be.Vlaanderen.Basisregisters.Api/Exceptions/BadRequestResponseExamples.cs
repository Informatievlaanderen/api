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
                ValidationErrors = new Dictionary<string, string[]>
                {
                    { "Voornaam", new[] {"Veld is verplicht." }},
                    { "Naam", new[] {"Veld mag niet kleiner zijn dan 4 karakters.", "Veld mag niet groter zijn dan 100 karakters." }}
                },
                ProblemInstanceUri = _problemDetailsHelper.GetInstanceUri(_httpContextAccessor.HttpContext)
            };
    }
}
