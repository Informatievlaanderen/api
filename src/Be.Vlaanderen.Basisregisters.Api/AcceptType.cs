namespace Be.Vlaanderen.Basisregisters.Api
{
    using System.Net.Mime;

    /// <summary>
    /// Enumeration of accept types
    /// </summary>
    public enum AcceptType
    {
        Json,
        JsonLd,
        Ld,
        JsonProblem,
        Xml,
        Atom,
        JsonCloudEventsBatch,
    }

    public static class AcceptTypes
    {
        public const string Any = "*/*";
        public const string Json = MediaTypeNames.Application.Json;
        public const string JsonLd = "application/ld+json";
        public const string Ld = "application/ld";
        public const string JsonProblem = MediaTypeNames.Application.ProblemJson;
        public const string Xml = MediaTypeNames.Application.Xml;
        public const string Atom = "application/atom+xml";
        public const string XmlProblem = MediaTypeNames.Application.ProblemXml;
        public const string JsonCloudEventsBatch = "application/cloudevents-batch+json";
    }
}
