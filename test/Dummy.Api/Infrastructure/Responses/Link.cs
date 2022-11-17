namespace Dummy.Api.Infrastructure.Responses
{
    using System.Runtime.Serialization;

    [DataContract(Name = "Link", Namespace = "")]
    public class Link
    {
        public static class Relations
        {
            public const string Home = "home";
            public const string ExampleAggregates = "example-aggregates";
        }

        [DataMember(Name = "Href", Order = 1)]
        public string Href { get; set; }

        [DataMember(Name = "Rel", Order = 2)]
        public string Rel { get; set; }

        [DataMember(Name = "Type", Order = 3)]
        public string Type { get; set; }

        // Make Swagger happy
        private Link() {}

        public Link(string href, string rel, string type)
        {
            Href = href;
            Rel = rel;
            Type = type;
        }
    }
}
