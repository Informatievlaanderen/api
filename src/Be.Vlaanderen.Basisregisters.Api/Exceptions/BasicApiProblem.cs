namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// Implementation of Problem Details for HTTP APIs https://tools.ietf.org/html/rfc7807
    /// </summary>
    public class BasicApiProblem
    {
        public static string DefaultTitle { get; } = "Er heeft zich een fout voorgedaan!";

        public static string GetProblemNumber() => $"{Guid.NewGuid():N}";

        public static string GetTypeUriFor<T>(T exception) where T : Exception
        {
            var name = typeof(T).Name.Replace("Exception", string.Empty);
            if (string.IsNullOrWhiteSpace(name))
                name = "Unknown";

            var assembly = System.Reflection.Assembly.GetCallingAssembly().GetName();
            return $"urn:{assembly.Name}:{name}".ToLowerInvariant();
        }

        /// <summary>URI referentie die het probleem type bepaalt.</summary>
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DataMember(Name = "type", Order = 100, EmitDefaultValue = false)]
        public string ProblemTypeUri { get; set; }

        /// <summary>Korte omschrijving van het probleem.</summary>
        [JsonProperty("title")]
        [DataMember(Name = "title", Order = 200, EmitDefaultValue = false)]
        public string Title { get; set; }

        /// <summary>Specifieke details voor dit probleem.</summary>
        [JsonProperty("detail")]
        [DataMember(Name = "detail", Order = 300, EmitDefaultValue = false)]
        public string Detail { get; set; }

        /// <summary>HTTP status code komende van de server voor dit probleem.</summary>
        [JsonProperty("status")]
        [DataMember(Name = "status", Order = 400, EmitDefaultValue = false)]
        public int HttpStatus { get; set; }

        /// <summary>URI naar de specifieke instantie van dit probleem.</summary>
        [JsonProperty("instance", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DataMember(Name = "instance", Order = 500, EmitDefaultValue = false)]
        public string ProblemInstanceUri { get; set; }
    }
}
