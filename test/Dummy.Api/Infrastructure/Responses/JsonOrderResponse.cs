namespace Dummy.Api.Infrastructure.Responses
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "JsonOrder", Namespace = "")]
    public class JsonOrderResponse
    {
        [DataMember(Name = "FieldD", Order = 4)]
        [JsonProperty(Required = Required.Default)]
        public string FieldD { get; set; } = "D";

        [DataMember(Name = "FieldA", Order = 1)]
        [JsonProperty(Required = Required.Default)]
        public string FieldA { get; set; } = "A";

        [DataMember(Name = "FieldC", Order = 3)]
        public string FieldC { get; set; } = "C";

        [DataMember(Name = "FieldB", Order = 2)]
        public string FieldB { get; set; } = "B";
    }

    public class JsonOrderResponseExamples : IExamplesProvider<JsonOrderResponse>
    {
        public JsonOrderResponse GetExamples() => new JsonOrderResponse();
    }
}
