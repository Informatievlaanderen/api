namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System.Linq;
    using System.Net;
    using System.Runtime.Serialization;
    using FluentValidation;
    using Newtonsoft.Json;

    /// <summary>
    /// Implementation of Problem Details for HTTP APIs https://tools.ietf.org/html/rfc7807 with additional Validation Errors
    /// </summary>
    public class BasicApiValidationProblem : BasicApiProblem
    {
        /// <summary>Validatie fouten.</summary>
        [JsonProperty("validationErrors", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DataMember(Name = "validationErrors", Order = 600, EmitDefaultValue = false)]
        public string[] ValidationErrors { get; set; }

        // Here to make DataContractSerializer happy
        public BasicApiValidationProblem() { }

        public BasicApiValidationProblem(ValidationException exception)
        {
            HttpStatus = (int)HttpStatusCode.BadRequest;
            Title = DefaultTitle;
            Detail = "Validatie mislukt!";
            ProblemInstanceUri = GetProblemNumber();
            ProblemTypeUri = GetTypeUriFor(exception);
            ValidationErrors = exception.Errors.Select(x => x.ErrorMessage).ToArray();
        }
    }
}
