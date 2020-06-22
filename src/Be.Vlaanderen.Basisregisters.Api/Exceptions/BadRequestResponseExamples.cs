namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System.Collections.Generic;
    using BasicApiProblem;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class BadRequestResponseExamples : IExamplesProvider<ValidationProblemDetails>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BadRequestResponseExamples(IHttpContextAccessor httpContextAccessor)
            => _httpContextAccessor = httpContextAccessor;

        public ValidationProblemDetails GetExamples() =>
            new ValidationProblemDetails
            {
                ValidationErrors = new Dictionary<string, string[]>
                {
                    { "Voornaam", new[] {"Veld is verplicht." }},
                    { "Naam", new[] {"Veld mag niet kleiner zijn dan 4 karakters.", "Veld mag niet groter zijn dan 100 karakters." }}
                },
                ProblemInstanceUri = _httpContextAccessor.HttpContext.GetProblemInstanceUri()
            };
    }
}
